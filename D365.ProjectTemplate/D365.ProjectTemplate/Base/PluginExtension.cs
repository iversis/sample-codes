using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk;

namespace Xrm.Base
{
    public static class PluginExtension
    {
        public static Entity GetFirstPostImage(this IPluginExecutionContext pluginContext)
        {            
            try
            {
                var obj = pluginContext.PostEntityImages.FirstOrDefault();
                return obj.Value;
            }
            catch
            {
                //do nothing
            }
            return null;
        }
        public static T GetFirstPostImage<T>(this IPluginExecutionContext pluginContext) where T : Entity
        {
            try
            {
                var obj = pluginContext.PostEntityImages.FirstOrDefault();
                return obj.Value.ToEntity<T>();
            }
            catch
            {
                //do nothing
            }
            return default;
        }

        public static Entity GetFirstPreImage(this IPluginExecutionContext pluginContext)
        {
            try
            {
                var obj = pluginContext.PreEntityImages.FirstOrDefault();
                return obj.Value;
            }
            catch
            {
                //do nothing
            }
            return null;
        }

        public static T GetFirstPreImage<T>(this IPluginExecutionContext pluginContext) where T : Entity
        {
            try
            {
                var obj = pluginContext.PreEntityImages.FirstOrDefault();
                return obj.Value.ToEntity<T>();
            }
            catch
            {
                //do nothing
            }
            return null;
        }

        public static Entity GetTargetImage(this IPluginExecutionContext pluginContext)
        {
            try
            {
                if (pluginContext.InputParameters.Contains("Target"))
                    return pluginContext.InputParameters["Target"] as Entity;
            }
            catch
            {
                //do nothing
            }
            return null;
        }
        public static T GetTargetImage<T>(this IPluginExecutionContext pluginContext) where T: Entity
        {
            try
            {
                if (pluginContext.InputParameters.Contains("Target"))
                    return (pluginContext.InputParameters["Target"] as Entity).ToEntity<T>();
            }
            catch
            {
                //do nothing
            }
            return null;
        }
    }
    
}
