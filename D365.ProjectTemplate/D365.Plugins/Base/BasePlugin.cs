using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xrm.Base
{
    public enum ExecutionMode
    {
        PreValidation = 10,
        PreOperation = 20,
        PostOperation = 40,
    }

    public class Step {        
        public string EntityName { get; set; }
        public string Message { get; set; }
        public ExecutionMode? Mode { get; set; }

        public Step (ExecutionMode mode, string message, string entityName = null)
        {
            this.Mode = mode;
            this.EntityName = entityName;
            this.Message = message;
        }

        public Step(string message)
        {
            this.Message = message;
        }

    }

    public abstract class BasePlugin : IPlugin
    {
        private IServiceProvider _serviceProvider;
        private IOrganizationServiceFactory _serviceFactory;
        private IPluginExecutionContext _pluginContext;
        private ITracingService _tracingService;
        private IOrganizationService _userOrgService;
        private IOrganizationService _sysOrgService;

        public IPluginExecutionContext PluginContext => _pluginContext;

        public IOrganizationService OrgService => _userOrgService ?? (_userOrgService = _serviceFactory.CreateOrganizationService(_pluginContext.UserId));

        public IOrganizationService SystemOrgService => _sysOrgService ?? (_sysOrgService = _serviceFactory.CreateOrganizationService(null));        

        public ITracingService TracingService => _tracingService ?? (_tracingService = (ITracingService)_serviceProvider.GetService(typeof(ITracingService)));

        public string MessageName => _pluginContext.MessageName;

        public ExecutionMode? Mode => (_pluginContext.Mode == 0 ? (ExecutionMode?)null : (ExecutionMode)_pluginContext.Mode);

        public void Execute(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceFactory = (IOrganizationServiceFactory)_serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            _pluginContext = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));
            
            //allow execution of pre-registered steps only.
            var registeredSteps = RegisterSteps();
            
            var step = registeredSteps.Where(s => {
                if ( (s.Mode != null || _pluginContext.Mode == 0 || s.Mode.Value == (ExecutionMode)_pluginContext.Mode)
                    && ((s.Message == null) || s.Message == _pluginContext.MessageName)
                    && ((s.EntityName == null) || s.EntityName == _pluginContext.PrimaryEntityName))
                    return true;
                return false;
            }).First();

            if (step != null)
                ExecutePlugin(_pluginContext);
        }

        public abstract Step[] RegisterSteps();

        public abstract void ExecutePlugin(IPluginExecutionContext context);
    }
}
