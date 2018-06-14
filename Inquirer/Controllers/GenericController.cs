using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Inquirer.Models;
using InquirerDLL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Inquirer.Controllers
{
    public class GenericController : Controller
    {
        public WebClient API { get; set; }
        public User CurrentUser
        {
            get
            {
                var json = User.FindFirst("User");
                return json != null ? JsonConvert.DeserializeObject<User>(json.Value) : null;
            }
        }
        public InquirerDLL.Output.Enumerations Enumerations
        {
            get
            {
                string s = HttpContext.Session.GetString("EnumerationCache");
                if (s != null)
                {
                    return JsonConvert.DeserializeObject<InquirerDLL.Output.Enumerations>(s);
                }
                else
                {
                    return null;
                }
            }
        }

        protected IConfiguration Configuration { get; set; }

        public GenericController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            API = new WebClient(Configuration, this);
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

        protected void Handle(Dictionary<string, string> errors, string defaultMessage = "При завантаженні даних сталася помилка.")
        {
            if (errors.Count > 0)
            {
                foreach (var e in errors)
                {
                    if (e.Key != "")
                    {
                        var prefix = ModelState.Keys
                            .Where(k => k.Contains(e.Key))
                            .FirstOrDefault();

                        prefix = prefix.Substring(0, prefix.LastIndexOf(e.Key));
                        ModelState.AddModelError(prefix + e.Key, e.Value);
                    }
                    else
                    {
                        ModelState.AddModelError("", e.Value);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", defaultMessage);
            }
        }

        protected async Task Authenticate(User user)
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim("User", JsonConvert.SerializeObject(user))
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(15)
            });
        }
    }
}
