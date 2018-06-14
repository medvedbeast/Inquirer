using InquirerAPI.PublicAPI.Models;
using InquirerAPI.Website.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InquirerAPI.PublicAPI.Infrastructure.Filters
{
    public class HandleExceptions : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var internalContext = (Website.Models.DatabaseContext)serviceProvider.GetService(typeof(Website.Models.DatabaseContext));
            var context = (DatabaseContext)serviceProvider.GetService(typeof(DatabaseContext));
            var logger = (ILogger)serviceProvider.GetService(typeof(ILogger));
            return new HandleExceptionsFilter(internalContext, context, logger);
        }
    }

    public class HandleExceptionsFilter : ExceptionFilterAttribute
    {
        protected Website.Models.DatabaseContext InternalDatabase { get; set; }
        protected DatabaseContext Database { get; set; }

        private ILogger Logger { get; set; }

        public HandleExceptionsFilter(Website.Models.DatabaseContext internalDatabase, DatabaseContext database, ILogger logger)
        {
            InternalDatabase = internalDatabase;
            Database = database;
            Logger = logger;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var result = new JsonResult(new
            {
                Message = context.Exception.InnerException?.Message ?? context.Exception.Message,
                StackTrace = context.Exception.InnerException?.StackTrace ?? context.Exception.StackTrace
            });
            result.StatusCode = StatusCodes.Status500InternalServerError;

            context.Result = result;
            context.ExceptionHandled = true;

            var token = context.HttpContext.Request.Cookies["token"];

            var userId = InternalDatabase.Key
                .Where(k => k.Content == token)
                .Select(k => k.UserId)
                .FirstOrDefault();

            var externalUserId = int.Parse(context.HttpContext.Request.Cookies["user-id"]);

            await Logger.Log(
                context.ActionDescriptor.DisplayName,
                false,
                context.HttpContext.Response.StatusCode,
                token,
                externalUserId,
                userId,
                context.Exception.InnerException.Message ?? context.Exception.Message
                );
        }
    }
}
