using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inquirer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inquirer.Controllers
{
    [Authorize]
    public class HomeController : GenericController
    {
        public HomeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            var user = CurrentUser;

            var statistics = await API.Get($"accounts/{CurrentUser.Id}/statistics", null);

            var surveys = await API.Get("surveys?isOpen=true", null);

            return View(new
            {
                User = new
                {
                    Id = (int)user.Id,
                    Name = user.Name,
                    Email = user.Email
                },
                Statistics = statistics.Content,
                Surveys = surveys.Content ?? new object[] { } 
            });
        }
    }
}
