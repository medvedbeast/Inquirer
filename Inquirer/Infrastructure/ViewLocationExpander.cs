using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Inquirer.Infrastructure
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.ContainsKey("rootAction"))
            {
                string action = context.Values["rootAction"];
                string[] locations = new string[] {
                    $"~/wwwroot/vue/components/{{1}}/{ action }/{{0}}.cshtml",
                    $"~/wwwroot/vue/components/shared/{{0}}.cshtml"
                };

                return locations.Union(viewLocations);
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var actionContext = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            string action = actionContext.ActionName.ToUnderscoreCase();
            context.Values["rootAction"] = action;
        }
    }
}
