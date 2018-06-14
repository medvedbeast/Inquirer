using System.Linq;
using System.Security.Cryptography;
using System.Text;
using InquirerAPI.Website.Data;
using InquirerAPI.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InquirerAPI.Website.Controllers
{
    public class GenericController : Controller
    {
        protected DatabaseContext Database { get; set; }
        protected IConfiguration Configuration { get; set; }
        protected User CurrentUser { get; set; }

        public GenericController(DatabaseContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Database = context;
            Configuration = configuration;

            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(httpContext.User.Claims.First(c => c.Type == "UserId").Value);
                CurrentUser = Database.User
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
            }
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
    }
}
