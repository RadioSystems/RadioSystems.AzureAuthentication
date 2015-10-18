namespace RadioSystems.AzureAuthentication.Constants {
    public sealed class DefaultAzureSettings {
        public static readonly string Tenant = "invisiblefence.com";
        public static readonly string ClientId = "5af0d675-5736-4e38-bd94-493118e422cf";
        public static readonly string ADInstance = "https://login.microsoft.com/{0}";
        public static readonly string AppName = "AuthModuleDemo";
        public static readonly string LogoutRedirectUri = "http://localhost:30321/OrchardLocal/";
        public static readonly bool BearerAuthEnabled = false;
        public static readonly bool SSLEnabled = false;
        public static readonly bool AzureWebSiteProtectionEnabled = false;
    }
}