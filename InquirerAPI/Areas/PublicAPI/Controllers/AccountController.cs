using InquirerAPI.PublicAPI.Data;
using InquirerAPI.PublicAPI.Infrastructure.Filters;
using InquirerAPI.PublicAPI.Models;
using InquirerDLL.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.PublicAPI.Controllers
{
    [HandleExceptions]
    [AuthorizeViaToken]
    [Area("PublicAPI")]
    [Produces("application/json")]
    [Route("public_api/accounts")]
    public class AccountController : GenericController
    {
        public AccountController(DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = Database.Users
                .Include(u => u.EducationProgress)
                .Include(u => u.EducationType)
                .Include(u => u.Group)
                .Include(u => u.Language)
                .Include(u => u.Location)
                .Include(u => u.Occupation)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return RequestError("Користувач не знайдений.");
            }

            return Result(new InquirerDLL.Entities.User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Sex = user.Sex,
                EducationProgressId = user.EducationProgress?.Id,
                EducationTypeId = user.EducationType?.Id,
                GroupId = user.Group?.Id,
                LanguageId = user.Language?.Id,
                LocationId = user.Location?.Id,
                OccupationId = user.Occupation?.Id
            });
        }

        [HttpGet("{id}/image")]
        public IActionResult GetImage(int id)
        {
            var user = Database.Users
                .Where(u => u.Id == id)
                .Select(u => (int?)u.Id)
                .FirstOrDefault();

            if (user == null)
            {
                return RequestError("Користувач не знайдений.");
            }

            string image = Database.Users
                .Where(u => u.Id == id)
                .Select(u => u.Image)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(image))
            {
                return RequestError("Image not found");
            }

            return Result(new
            {
                image
            });
        }

        [HttpGet("{id}/statistics")]
        public IActionResult GetStatistics(int id)
        {
            var user = Database.Users
               .Where(u => u.Id == id)
               .Select(u => (int?)u.Id)
               .FirstOrDefault();

            if (user == null)
            {
                return RequestError("Користувач не знайдений.");
            }

            var quantity = Database.Surveys
                .Where(s => s.CreatorId == id)
                .Count();

            var open = Database.Surveys
                .Where(s => s.CreatorId == id && s.IsOpen == true)
                .Count();

            var closed = Database.Surveys
                .Where(s => s.CreatorId == id && s.IsOpen == false)
                .Count();

            var answers = Database.Answers
                .Include(a => a.Question)
                    .ThenInclude(q => q.Survey)
                .Where(a => a.Question.Survey.CreatorId == id)
                .Select(a => a.RespondentId)
                .Distinct()
                .Count();

            return Result(new InquirerDLL.Output.Account.Statistics
            {
                Quantity = quantity,
                Open = open,
                Closed = closed,
                Answers = answers
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]InquirerDLL.Entities.User model, int id)
        {
            if (CurrentUser == null || CurrentUser.Id != id)
            {
                return AuthenticationError();
            }

            CurrentUser.Name = model.Name;
            CurrentUser.Email = model.Email;
            if (model.Password != null)
            {
                CurrentUser.Password = Hash(model.Password);
            }
            CurrentUser.Sex = model.Sex;
            CurrentUser.EducationTypeId = model.EducationTypeId;
            CurrentUser.EducationProgressId = model.EducationProgressId;
            CurrentUser.LanguageId = model.LanguageId;
            CurrentUser.LocationId = model.LocationId;
            CurrentUser.OccupationId = model.OccupationId;

            await Database.SaveChangesAsync();

            return Get(id);
        }

        [HttpPut("{id}/image")]
        public async Task<IActionResult> UpdateImage([FromBody]InquirerDLL.Entities.User model, int id)
        {
            if (CurrentUser == null || CurrentUser.Id != id)
            {
                return AuthenticationError();
            }

            CurrentUser.Image = model.Image;

            await Database.SaveChangesAsync();

            return Get(id);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]InquirerDLL.Entities.User model)
        {
            string password = Hash(model.Password);
            User user = await Database.Users
                .Include(u => u.EducationProgress)
                .Include(u => u.EducationType)
                .Include(u => u.Group)
                .Include(u => u.Language)
                .Include(u => u.Location)
                .Include(u => u.Occupation)
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Користувач з такою комбінацією логіну та паролю не знайдений.");
                return ValidationError(ModelState);
            }

            return Get(user.Id);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]InquirerDLL.Entities.User model)
        {
            var user = new User
            {
                Email = model.Email,
                Password = model.Password != null ? Hash(model.Password) : "",
                Name = model.Name,
                GroupId = (int)USER_GROUP.USER
            };

            Database.Users.Add(user);

            await Database.SaveChangesAsync();

            return Get(user.Id);
        }
    }
}
