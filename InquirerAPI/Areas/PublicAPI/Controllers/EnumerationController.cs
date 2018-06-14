using InquirerAPI.PublicAPI.Infrastructure.Filters;
using InquirerAPI.PublicAPI.Models;
using InquirerDLL.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace InquirerAPI.PublicAPI.Controllers
{
    [AuthorizeViaToken]
    [HandleExceptions]
    [Area("PublicAPI")]
    [Produces("application/json")]
    [Route("public_api/enumerations")]
    public class EnumerationController : GenericController
    {
        public EnumerationController(DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            var educationTypes = Database.UserEducationTypes
                .Select(t => new InquirerDLL.Entities.UserEducationType
                {
                    Id = t.Id,
                    Name = t.Name
                });

            var educationProgresses = Database.UserEducationProgresses
                .Select(p => new InquirerDLL.Entities.UserEducationProgress
                {
                    Id = p.Id,
                    Name = p.Name
                });

            var sexes = new InquirerDLL.Entities.UserSex[]
            {
                new InquirerDLL.Entities.UserSex{
                    Id = SEX.MALE,
                    Name = "Чоловіча"
                },
                new InquirerDLL.Entities.UserSex{
                    Id = SEX.FEMALE,
                    Name = "Жіноча"
                },
            };

            var languages = Database.UserLanguages
                .Select(l => new InquirerDLL.Entities.UserLanguage
                {
                    Id = l.Id,
                    Name = l.Name
                });

            var locations = Database.UserLocations
                .Select(l => new InquirerDLL.Entities.UserLocation
                {
                    Id = l.Id,
                    Name = l.Name
                });

            var occupations = Database.UserOccupations
                .Select(o => new InquirerDLL.Entities.UserOccupation
                {
                    Id = o.Id,
                    Name = o.Name
                });

            var questionTypes = Database.QuestionTypes
                .Select(t => new InquirerDLL.Entities.QuestionType
                {
                    Id = t.Id,
                    Name = t.Name
                });

            var userGroups = Database.UserGroups
                .Select(g => new InquirerDLL.Entities.UserGroup
                {
                    Id = g.Id,
                    Name = g.Name
                });

            return Result(new InquirerDLL.Output.Enumerations
            {
                EducationProgresses = educationProgresses,
                EducationTypes = educationTypes,
                Languages = languages,
                Locations = locations,
                Occupations = occupations,
                Sexes = sexes,
                QuestionTypes = questionTypes,
                UserGroups = userGroups
            });
        }
    }
}
