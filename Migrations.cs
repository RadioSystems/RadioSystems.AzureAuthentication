using Orchard.Data.Migration;

namespace RadioSystems.AzureAuthentication {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("AzureSettingsPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("Tenant")
                .Column<string>("ADInstance")
                .Column<string>("ClientId")
                .Column<string>("AppName")
                .Column<string>("LogoutRedirectUri")
                .Column<bool>("BearerAuthEnabled")
                .Column<bool>("SSLEnabled")
                .Column<bool>("AzureWebSiteProtectionEnabled")
            );

            return 1;
        }
    }
}