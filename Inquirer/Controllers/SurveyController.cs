using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Inquirer.Controllers
{
    [Authorize]
    public class SurveyController : GenericController
    {
        public SurveyController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
        }

        [AllowAnonymous]
        [HttpGet("[controller]/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var response = await API.Get<InquirerDLL.Entities.Survey>($"surveys/{id}", null);

            return View(new
            {
                Id = (int)response.Content.Id,
                Title = response.Content.Title,
                Description = response.Content.Description,
                StartDate = response.Content.StartDate,
                EndDate = response.Content.EndDate,
                IsOpen = (bool)response.Content.IsOpen,
                IsAuthenticationRequired = (bool)response.Content.IsAuthenticationRequired,
                CreatorId = response.Content.CreatorId,
                Questions = response.Content.Questions,
                User = new
                {
                    Id = CurrentUser != null ? CurrentUser.Id : null,
                    Email = CurrentUser != null ? CurrentUser.Email : null,
                    Name = CurrentUser != null ? CurrentUser.Name : null
                }
            });
        }

        public async Task<IActionResult> Create(string returnUrl)
        {
            var response = await API.Post<object>("surveys", CurrentUser.Id);

            if (response.IsSuccessfull)
            {
                return RedirectToAction("Edit", "Survey", new { id = response.Content });
            }

            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await API.Get<InquirerDLL.Entities.Survey>($"surveys/{id}", null);

            return View(new
            {
                Id = (int)response.Content.Id,
                Title = response.Content.Title,
                Description = response.Content.Description,
                StartDate = response.Content.StartDate,
                EndDate = response.Content.EndDate,
                IsOpen = (bool)response.Content.IsOpen,
                IsAuthenticationRequired = (bool)response.Content.IsAuthenticationRequired,
                Questions = response.Content.Questions,
                User = new
                {
                    Id = (int)CurrentUser.Id,
                    Email = CurrentUser.Email,
                    Name = CurrentUser.Name
                }
            });
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> Statistics(int id)
        {
            var response = await API.Get<InquirerDLL.Output.Survey.Statistics>($"surveys/{id}/statistics", null);

            return View(new
            {
                Survey = new
                {
                    Id = id
                },
                Statistics = response.Content,
                User = new
                {
                    Id = (int)CurrentUser.Id,
                    Email = CurrentUser.Email,
                    Name = CurrentUser.Name
                }
            });
        }
    }
}
