using Orchard.ContentManagement;

namespace RadioSystems.AzureAuthentication.Models {
    public class AzureSettingsPart : ContentPart<AzureSettingsPartRecord> {
        public string Tenant {
            get { return Retrieve(x => x.Tenant); }
            set { Store(x => x.Tenant, value); }
        }

        public string ADInstance {
            get { return Retrieve(x => x.ADInstance); }
            set { Store(x => x.ADInstance, value); }
        }

        public string ClientId {
            get { return Retrieve(x => x.ClientId); }
            set { Store(x => x.ClientId, value); }
        }

        public string AppName {
            get { return Retrieve(x => x.AppName); }
            set { Store(x => x.AppName, value); }
        }

        public string LogoutRedirectUri {
            get { return Retrieve(x => x.LogoutRedirectUri); }
            set { Store(x => x.LogoutRedirectUri, value); }
        }

        public bool BearerAuthEnabled {
            get { return Retrieve(x => x.BearerAuthEnabled); }
            set { Store(x => x.BearerAuthEnabled, value); }
        }

        public bool SSLEnabled {
            get { return Retrieve(x => x.SSLEnabled); }
            set { Store(x => x.SSLEnabled, value); }
        }

        public bool AzureWebSiteProtectionEnabled {
            get { return Retrieve(x => x.AzureWebSiteProtectionEnabled); }
            set { Store(x => x.AzureWebSiteProtectionEnabled, value); }
        }
    }
}