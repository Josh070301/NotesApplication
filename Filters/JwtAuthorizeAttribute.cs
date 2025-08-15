using NotesApplication.Helpers;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace NotesApplication.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class JwtAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0)
            {
                return; // Skip authorization for actions with [AllowAnonymous]
            }

            var cookie = filterContext.HttpContext.Request.Cookies["auth_token"];
            if (cookie == null)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            var principal = JwtHelper.ValidateToken(cookie.Value);
            if (principal == null)
            {
                HandleUnauthorizedRequest(filterContext);
                return;
            }

            // Set the principal for the current request
            filterContext.HttpContext.User = principal;
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "Login" },
                    { "returnUrl", filterContext.HttpContext.Request.Url.PathAndQuery }
                });
        }
    }
}