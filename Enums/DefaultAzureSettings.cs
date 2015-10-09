namespace RadioSystems.AzureAuthentication.Enums {
    public sealed class DefaultAzureSettings {
        public static readonly string Tenant = "https://mytenant.com";
        public static readonly string ClientId = "MyClientId";
        public static readonly string ADInstance = "https://login.microsoftonline.com/{0}";
        public static readonly string AppName = "MyAppName";
        public static readonly string LogoutRedirectUri = "http://localhost:30321/OrchardLocal/";
        public static readonly bool BearerAuthEnabled = false;
        public static readonly bool SSLEnabled = false;
        public static readonly bool AzureWebSiteProtectionEnabled = false;
    }
}