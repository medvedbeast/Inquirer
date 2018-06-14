using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Http;
using Inquirer.Controllers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Inquirer.Infrastructure.Attributes
{
    public class RequireEnumerations : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = context.HttpContext.Session.GetString("EnumerationCache");
            if (string.IsNullOrEmpty(cache))
            {
                var controller = context.Controller as GenericController;
                var response = await controller.API.Get<InquirerDLL.Output.Enumerations>("enumerations", null);
                context.HttpContext.Session.SetString("EnumerationCache", JsonConvert.SerializeObject(response.Content));
            }
            await next();
        }
    }
}
