using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization;
using RadioSystems.AzureAuthentication.Models;

namespace RadioSystems.AzureAuthentication.Handlers {
    public class AzureSettingsPartHandler : ContentHandler {
        public Localizer T { get; set; }
        
        public AzureSettingsPartHandler() {
            T = NullLocalizer.Instance;
        }

        public AzureSettingsPartHandler(IRepository<AzureSettingsPartRecord> azureSettingsRepository) {
            Filters.Add(StorageFilter.For(azureSettingsRepository));
            Filters.Add(new ActivatingFilter<AzureSettingsPart>("Site"));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site") {
                return;
            }

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Azure")));
        }
    }
}