using InquirerAPI.PublicAPI.Data;
using InquirerAPI.PublicAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace InquirerAPI.PublicAPI.Controllers
{
    public class GenericController : Controller
    {
        public User CurrentUser { get; set; }

        protected DatabaseContext Database { get; set; }
        protected IConfiguration Configuration { get; set; }

        public GenericController(DatabaseContext context, IConfiguration configuration)
        {
            Database = context;
            Configuration = configuration;
        }

        public string Hash(object target)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes($"{target}"));
            string result = "";
            foreach (var b in hash)
            {
                result += $"{b:x2}";
            }
            return result;
        }

        public IActionResult ValidationError(ModelStateDictionary errors)
        {
            var result = new List<object>();

            foreach (var pair in errors)
            {
                if (pair.Value.Errors != null && pair.Value.Errors.Count > 0)
                {
                    foreach (var e in pair.Value.Errors)
                    {
                        result.Add(new
                        {
                            field = pair.Key == "" ? "model" : pair.Key.ToLower(),
                            msg = e.ErrorMessage
                        });
                    }
                }
            }

            JsonResult jsonResult = new JsonResult(result);
            jsonResult.StatusCode = StatusCodes.Status400BadRequest;

            return jsonResult;
        }

        public IActionResult Result(object data)
        {
            JsonResult jsonResult = new JsonResult(data);
            jsonResult.StatusCode = StatusCodes.Status200OK;

            return jsonResult;
        }

        public IActionResult AuthenticationError()
        {
            var result = new List<object>();
            result.Add(new
            {
                field = "model",
                msg = "Ви не авторизовані на виконання цієї дії."
            });

            JsonResult jsonResult = new JsonResult(result);
            jsonResult.StatusCode = StatusCodes.Status401Unauthorized;

            return jsonResult;
        }

        public IActionResult RequestError(string message = "")
        {
            var result = new List<object>();
            result.Add(new
            {
                field = "model",
                msg = (message.Length > 0 ? message : "Помилка при виконанні запиту.")
            });

            JsonResult jsonResult = new JsonResult(result);
            jsonResult.StatusCode = StatusCodes.Status400BadRequest;

            return jsonResult;
        }

        public IActionResult Status(bool status)
        {
            var result = new
            {
                status
            };

            JsonResult jsonResult = new JsonResult(result);
            jsonResult.StatusCode = status ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;

            return jsonResult;
        }
    }
}
