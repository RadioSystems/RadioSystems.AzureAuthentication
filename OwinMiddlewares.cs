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
using Orchard.Settings;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Logging;
using Orchard.Owin;
using Owin;
using RadioSystems.AzureAuthentication.Enums;
using RadioSystems.AzureAuthentication.Models;
using RadioSystems.AzureAuthentication.Security;

namespace RadioSystems.AzureAuthentication {
    public class OwinMiddlewares : IOwinMiddlewareProvider {
        public ILogger Logger { get; set; }

        private readonly ISiteService _siteService;

        private static readonly string _defaultTenant = "https://mytenant.com";
        private static readonly string _defaultClientId = "MyClientId";
        private static readonly string _defaultADInstance = "https://login.microsoftonline.com/{0}";
        private static readonly string _defaultAppName = "MyAppName";

        public OwinMiddlewares(ISiteService siteService) {
            Logger = NullLogger.Instance;

            _siteService = siteService;
        }

        public IEnumerable<OwinMiddlewareRegistration> GetOwinMiddlewares() {
            var middlewares = new List<OwinMiddlewareRegistration>();

            var site = _siteService.GetSiteSettings();

            var settings = site.As<AzureSettingsPart>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            var openIdOptions = new OpenIdConnectAuthenticationOptions {
                ClientId = settings.ClientId ?? _defaultClientId,
                Authority = string.Format(CultureInfo.InvariantCulture, settings.ADInstance ?? _defaultADInstance, settings.Tenant ?? _defaultTenant),
                PostLogoutRedirectUri = settings.LogoutRedirectUri ?? site.BaseUrl,
                Notifications = new OpenIdConnectAuthenticationNotifications()
            };

            var cookieOptions = new CookieAuthenticationOptions();

            var bearerAuthOptions = new WindowsAzureActiveDirectoryBearerAuthenticationOptions {
                //TODO: set this to https if ssl enabled settings is true
                TokenValidationParameters = new TokenValidationParameters {
                    ValidAudience = string.Format("http://{0}/{1}", settings.Tenant ?? _defaultTenant, settings.AppName ?? _defaultAppName)
                },
                Tenant = _defaultTenant,
                AuthenticationType = "Oauth2Bearer"
            };

            if (_defaultWebSiteProtectionEnabled) {
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