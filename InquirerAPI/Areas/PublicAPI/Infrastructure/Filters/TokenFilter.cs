using InquirerAPI.PublicAPI.Controllers;
using InquirerAPI.PublicAPI.Models;
using InquirerAPI.Website.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.PublicAPI.Infrastructure.Filters
{
    public class AuthorizeViaToken : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var internalContext = (Website.Models.DatabaseContext)serviceProvider.GetService(typeof(Website.Models.DatabaseContext));
            var context = (DatabaseContext)serviceProvider.GetService(typeof(DatabaseContext));
            var logger = (ILogger)serviceProvider.GetService(typeof(ILogger));
            return new TokenRequiredFilter(internalContext, context, logger);
        }
    }

    public class TokenRequiredFilter : ActionFilterAttribute
    {
        protected Website.Models.DatabaseContext InternalDatabase { get; set; }
        protected DatabaseContext Database { get; set; }

        private GenericController Controller { get; set; }
        private ILogger Logger { get; set; }

        public TokenRequiredFilter(Website.Models.DatabaseContext internalDatabase, DatabaseContext database, ILogger logger)
        {
            InternalDatabase = internalDatabase;
            Database = database;
            Logger = logger;
        }

        public bool ValidateModel { get; set; } = true;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!BeforeAction(context))
            {
                await AfterAction(context, "Input data validation error.");
                return;
            }
            await next();
            await AfterAction(context);
        }

        public bool BeforeAction(ActionExecutingContext context)
        {
            Controller = context.Controller as GenericController;

            var token = context.HttpContext.Request.Cookies["token"];
            var userId = context.HttpContext.Request.Cookies["user-id"];

            int tokenCount = InternalDatabase.Key
                .Where(k => k.Content == token)
                .Count();

            if (tokenCount != 1)
            {
                context.Result = Controller.AuthenticationError();
            }

            if (userId != null)
            {
                var id = int.Parse(userId);

                var user = Database.Users
                    .Where(u => u.Id == id)
                    .FirstOrDefault();

                if (user != null)
                {
                    Controller.CurrentUser = user;
                }
            }

            if (ValidateModel && !context.ModelState.IsValid)
            {
                context.Result = Controller.ValidationError(context.ModelState);
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task AfterAction(ActionExecutingContext context, string message = null)
        {
            if (Controller.CurrentUser != null)
            {
                var token = context.HttpContext.Request.Cookies["token"];

                var userId = InternalDatabase.Key
                    .Where(k => k.Content == token)
                    .Select(k => k.UserId)
                    .FirstOrDefault();

                await Logger.Log(
                    context.ActionDescriptor.DisplayName,
                    context.HttpContext.Response.StatusCode == StatusCodes.Status200OK && message == null,
                    context.HttpContext.Response.StatusCode,
                    token,
                    Controller.CurrentUser.Id,
                    userId,
                    message
                    );
            }
        }
    }
}
