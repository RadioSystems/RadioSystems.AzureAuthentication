using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OpenIdConnect;
using Orchard.Logging;
using Orchard.Owin;
using Owin;
using RadioSystems.AzureAuthentication.Constants;
using RadioSystems.AzureAuthentication.Security;

namespace RadioSystems.AzureAuthentication {
    public class OwinMiddlewares : IOwinMiddlewareProvider {
        public ILogger Logger { get; set; }

        private readonly string _azureClientId = DefaultAzureSettings.ClientId;
        private readonly string _azureTenant = DefaultAzureSettings.Tenant;
        private readonly string _azureADInstance = DefaultAzureSettings.ADInstance;
        private readonly string _logoutRedirectUri = DefaultAzureSettings.LogoutRedirectUri;
        private readonly string _azureAppName = DefaultAzureSettings.AppName;
        private readonly bool _sslEnabled = DefaultAzureSettings.SSLEnabled;
        private readonly bool _azureWebSiteProtectionEnabled = DefaultAzureSettings.AzureWebSiteProtectionEnabled;

        public OwinMiddlewares() {
            Logger = NullLogger.Instance;
        }

        public IEnumerable<OwinMiddlewareRegistration> GetOwinMiddlewares() {
            var middlewares = new List<OwinMiddlewareRegistration>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            var openIdOptions = new OpenIdConnectAuthenticationOptions {
                ClientId = _azureClientId,
                Authority = string.Format(CultureInfo.InvariantCulture, _azureADInstance, _azureTenant),
                PostLogoutRedirectUri = _logoutRedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications ()
            };

            var cookieOptions = new CookieAuthenticationOptions();

            var bearerAuthOptions = new WindowsAzureActiveDirectoryBearerAuthenticationOptions {
                TokenValidationParameters = new TokenValidationParameters {
                    ValidAudience = string.Format(_sslEnabled ? "https://{0}/{1}" : "http://{0}/{1}", _azureTenant, _azureAppName)
                }
            };

            if (_azureWebSiteProtectionEnabled) {
                middlewares.Add(new OwinMiddlewareRegistration {
                    Priority = "9",
                    Configure = app => { app.SetDataProtectionProvider(new MachineKeyProtectionProvider()); }
                });
            }

            middlewares.Add(new OwinMiddlewareRegistration {
                Priority = "10",
                Configure = app => {
                    app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

                    app.UseCookieAuthentication(cookieOptions);

                    app.UseOpenIdConnectAuthentication(openIdOptions);

                    //This is throwing an XML DTD is prohibited error?
                    //app.UseWindowsAzureActiveDirectoryBearerAuthentication(bearerAuthOptions);
                }
            });

            return middlewares;
        }
    }
}