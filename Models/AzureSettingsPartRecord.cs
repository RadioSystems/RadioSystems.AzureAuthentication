using Orchard.ContentManagement.Records;

namespace RadioSystems.AzureAuthentication.Models {
    public class AzureSettingsPartRecord : ContentPartRecord {
        public virtual string Tenant { get; set; }
        public virtual string ADInstance { get; set; }
        public virtual string ClientId { get; set; }
        public virtual string AppName { get; set; }
        public virtual string LogoutRedirectUri { get; set; }
        public virtual bool BearerAuthEnabled { get; set; }
        public virtual bool SSLEnabled { get; set; }
        public virtual bool AzureWebSiteProtectionEnabled { get; set; }
    }
}