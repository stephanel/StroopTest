using Microsoft.AspNetCore.Mvc;

namespace StroopTest.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string GetAction(this IUrlHelper urlHelper,
            string actionName, 
            string controllerName, 
            object routeValues = null)
        {
            string scheme = urlHelper.ActionContext.HttpContext.Request.Scheme;
            return urlHelper.Action(actionName, controllerName, routeValues, scheme);
        }     
    }
}