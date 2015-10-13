using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OpenIdConnect;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment;
using Orchard.Logging;
using Orchard.Owin;
using Owin;
using RadioSystems.AzureAuthentication.Constants;
using RadioSystems.AzureAuthentication.Models;
using RadioSystems.AzureAuthentication.Security;

namespace RadioSystems.AzureAuthentication {
    public class OwinMiddlewares : IOwinMiddlewareProvider {
        public ILogger Logger { get; set; }


        private readonly string _azureClientId = DefaultAzureSettings.ClientId;
        private readonly string _azureTenant = DefaultAzureSettings.Tenant;
        private readonly string _azureADInstance = DefaultAzureSettings.ADInstance;
        private readonly string _logoutRedirectUri = DefaultAzureSettings.LogoutRedirectUri;
        private readonly string _azureAdInstance = DefaultAzureSettings.ADInstance;
        private readonly string _azureAppName = DefaultAzureSettings.AppName;
        private readonly bool _sslEnabled = DefaultAzureSettings.SSLEnabled;
        private readonly bool _azureWebSiteProtectionEnabled = DefaultAzureSettings.AzureWebSiteProtectionEnabled;

        public OwinMiddlewares(IRepository<AzureSettingsPartRecord> azureSettingRepository) {
            Logger = NullLogger.Instance;

            try {
                var settings = azureSettingRepository.Table.FirstOrDefault();

                if (settings == null) {
                    return;
                }

                _azureClientId = settings.ClientId ?? _azureClientId;
                _azureTenant = settings.Tenant ?? _azureTenant;
                _azureAdInstance = settings.ADInstance ?? _azureADInstance;
                _azureAppName = settings.AppName ?? _azureAppName;
                _logoutRedirectUri = settings.LogoutRedirectUri ?? _logoutRedirectUri;
                _sslEnabled = settings.SSLEnabled;
                _azureWebSiteProtectionEnabled = settings.AzureWebSiteProtectionEnabled;
            }
            catch (Exception ex) {
                Logger.Log(LogLevel.Debug, ex, "An error occured while accessing azure settings: {0}");
            }
        }

        public IEnumerable<OwinMiddlewareRegistration> GetOwinMiddlewares() {
            var middlewares = new List<OwinMiddlewareRegistration>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            var openIdOptions = new OpenIdConnectAuthenticationOptions {
                ClientId = _azureClientId,
                Authority = string.Format(CultureInfo.InvariantCulture, _azureAdInstance, _azureTenant),
                PostLogoutRedirectUri = _logoutRedirectUri,
                Notifications = new OpenIdConnectAuthenticationNotifications {
                    SecurityTokenValidated = (context) => {
                        try {
                            //We should be able assign roles based on claims here
                            return Task.FromResult(0);
                        }
                        catch (SecurityTokenValidationException ex) {
                            return Task.FromResult(0);
                        }
                    }
                }
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