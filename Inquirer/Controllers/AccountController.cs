using System.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Inquirer.Infrastructure.Attributes;
using Inquirer.Models;
using InquirerDLL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inquirer.Controllers
{
    [Authorize]
    public class AccountController : GenericController
    {
        public AccountController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
        }

        [RequireEnumerations]
        public IActionResult Edit()
        {
            var user = CurrentUser;

            var enumerations = Enumerations;

            return View(new
            {
                Id = (int)user.Id,
                Name = user.Name,
                Email = user.Email,
                Sex = user.Sex,
                EducationProgress = user.EducationProgressId,
                EducationType = user.EducationTypeId,
                Language = user.LanguageId,
                Occupation = user.OccupationId,
                Location = user.LocationId,
                Enumerations = enumerations
            });
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            dynamic model = new ExpandoObject();
            model.returnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]User model)
        {
            var response = await API.Post<User>("accounts/login", model);

            if (response.IsSuccessfull)
            {
                await Authenticate(response.Content);
            }

            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> Authorize([FromBody]User model)
        {
            await Authenticate(model);
            return Json(new RequestResult<object>
            {
                IsSuccessfull = true
            });
        }

        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            dynamic model = new ExpandoObject();
            model.returnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]User model)
        {
            var response = await API.Post<User>("accounts/register", model);

            if (response.IsSuccessfull)
            {
                await Authenticate(response.Content);
            }

            return Json(response);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
