using InquirerAPI.Website.Models;
using InquirerAPI.Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace InquirerAPI.Website.Controllers
{
    [Authorize]
    [Area("Website")]
    public class HomeController : GenericController
    {
        public HomeController(DatabaseContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            var user = new
            {
                Id = CurrentUser.Id,
                Name = CurrentUser.Name,
                Email = CurrentUser.Email,
                RegisteredOn = CurrentUser.RegisteredOn,
                LastSeenOn = CurrentUser.LastSeenOn
            };

            var keyTypes = Database.KeyType
                .Select(t => new
                {
                    Id = t.Id,
                    Name = t.Name
                });

            var activitiesCount = Database.Activity
                .Where(a => a.UserId == CurrentUser.Id)
                .Count();

            return View(new
            {
                User = user,
                KeyTypes = keyTypes,
                ActivitiesCount = activitiesCount
            });
        }
    }
}
