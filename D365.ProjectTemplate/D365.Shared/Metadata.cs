using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace D365.Shared
{
    public static class Metadata
    {
        public static string GetOptionSetLabel(IOrganizationService service, string entityName, string optionSetName, int value)
        {
            var key = $"OPTION-{entityName}-{optionSetName}";

            //retrieve from cache if available
            var picklist = MemoryCache.GetItem<PicklistAttributeMetadata>(key, () =>
            {
                RetrieveAttributeRequest req = new RetrieveAttributeRequest()
                {
                    EntityLogicalName = entityName,
                    LogicalName = optionSetName
                };

                RetrieveAttributeResponse resp = service.Execute(req) as RetrieveAttributeResponse;
                return (PicklistAttributeMetadata)resp.AttributeMetadata;
            });

            //find the option set and return the label.
            foreach (var opt in picklist.OptionSet.Options)
            {
                if (opt.Value == value)
                    return opt.Label.UserLocalizedLabel.Label;
            }

            return null;
        }

        public static string GetEntityPrimaryAttribute(IOrganizationService service, string entityName)
        {
            var key = $"ENTITY-{entityName}";

            //retrieve from cache if available
            var entityMeta = MemoryCache.GetItem<EntityMetadata>(key, () =>
            {
                RetrieveEntityRequest req = new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.Attributes,
                    LogicalName = entityName,
                };

                RetrieveEntityResponse resp = service.Execute(req) as RetrieveEntityResponse;
                return resp.EntityMetadata;
            });

            //return the primary name 
            return entityMeta.PrimaryNameAttribute;
        }
    }
}
