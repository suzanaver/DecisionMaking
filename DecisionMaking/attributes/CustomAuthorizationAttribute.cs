using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DecisionMaking.attributes
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                // no user is authenticated => no need to go any further
                return false;
            }

            // at this stage we have an authenticated user
            string username = httpContext.User.Identity.Name.ToLower();

            return IsInRole(username, this.Roles);
        }

        private bool IsInRole(string username, string roles)
        {
            username = username.Replace("hotmobile\\", "");
            username = username.Replace("phi-networks\\", "");
            if (aloowedUsers.Contains("phi-networks\\" + username) || aloowedUsers.Contains(username))
            {
                return true;
            }
            else
            {
                return false;
            }

        
        }
        private List<string> aloowedUsers = new List<string>()
        {
          "shlomo.dvora","ram-yona.levy","kobi.tashbini","nitzan.eliyahu","claudine.segev","kobita","erez.sela"

        };

        //
        // Summary:
        //     Called when a process requests authorization.
        //
        // Parameters:
        //   filterContext:
        //     The filter context, which encapsulates information for using System.Web.Mvc.AuthorizeAttribute.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The filterContext parameter is null.
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            base.HandleUnauthorizedRequest(filterContext);
        }


    }
}