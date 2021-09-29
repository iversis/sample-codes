using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Xrm.Base;

namespace D365.ProjectTemplate.Plugins.Entities.Account
{
    public class SamplePlugin : BasePlugin
    {
        public override Step[] RegisterSteps()
        {
            return new Step[]
            {
                new Step(ExecutionMode.PostOperation, "Create", "account"),
                new Step(ExecutionMode.PostOperation, "Update", "account"),
            };
        }

        public override void ExecutePlugin(IPluginExecutionContext context)
        {
            var target = context.GetTargetImage();

        }

       
    }
}
