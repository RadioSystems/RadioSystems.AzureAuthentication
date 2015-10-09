using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace RadioSystems.AzureAuthentication {
    public class Routes : IRouteProvider {
        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                new RouteDescriptor {
                    Route = new Route(
                        "Users/Account/{action}",
                        new RouteValueDictionary {
                            {"area", "RadioSystems.AzureAuthentication"},
                            {"controller", "Account"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "RadioSystems.AzureAuthentication"}
                        },
                        new MvcRouteHandler())
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var route in GetRoutes()) {
                routes.Add(route);
            }
        }
    }
}