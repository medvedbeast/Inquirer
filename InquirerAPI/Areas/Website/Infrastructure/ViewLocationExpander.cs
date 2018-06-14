using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.Website.Infrastructure
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string[] locations;

            var action = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            if (action != null)
            {
                locations = new string[] {
                $"Areas/{{2}}/Components/{{1}}/{action.ActionName}/{{0}}.cshtml",
                "Areas/{2}/Components/Shared/{0}.cshtml"
            };
            }
            else
            {
                locations = new string[] {
                "Areas/{2}/Components/{1}/{0}.cshtml",
                "Areas/{2}/Components/Shared/{0}.cshtml"
            };
            }
            
            return locations.Union(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["customviewlocation"] = nameof(ViewLocationExpander);
        }
    }
}
