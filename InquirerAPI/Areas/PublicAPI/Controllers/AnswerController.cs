using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InquirerAPI.PublicAPI.Controllers;
using InquirerAPI.PublicAPI.Data;
using InquirerAPI.PublicAPI.Infrastructure.Filters;
using InquirerAPI.PublicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InquirerAPI.PublicAPI.Controllers
{
    [HandleExceptions]
    [AuthorizeViaToken]
    [Area("PublicAPI")]
    [Produces("application/json")]
    [Route("public_api/answers")]
    public class AnswerController : GenericController
    {
        public AnswerController(DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody]IEnumerable<InquirerDLL.Entities.Answer> model, int surveyId, int userId)
        {
            User user = null;

            if (userId != -1)
            {
                user = Database.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
            }

            var survey = Database.Surveys
                .Where(s => s.Id == surveyId)
                .FirstOrDefault();

            if (survey.IsAuthenticationRequired)
            {
                if (user == null)
                {
                    return RequestError("Опитування доступне лише зареєстрованим користувачам.");
                }
                else
                {
                    var answers = Database.Answers
                        .Include(a => a.Respondent)
                        .Include(a => a.Question)
                        .Where(a => a.Question.SurveyId == surveyId && a.Respondent.AssociatedUserId == userId)
                        .Select(a => a.Id)
                        .Count();

                    if (answers > 0)
                    {
                        return RequestError("Опитування можна пройти лише один раз.");
                    }
                }
            }

            string userAgent = "";
            var agentHeaders = HttpContext.Request.Headers["User-Agent"];
            if (agentHeaders.Count > 0)
            {
                userAgent = agentHeaders[0];
            }

            string ipAddress = HttpContext.Connection.RemoteIpAddress + " / " + HttpContext.Connection.LocalIpAddress;
            var addressHeaders = HttpContext.Request.Headers["X-Forwarded-For"];
            if (addressHeaders.Count > 0)
            {
                ipAddress = addressHeaders[0];
            }

            var respondent = new Respondent
            {
                IpAddress = ipAddress,
                UserAgent = userAgent,
                AssociatedUserId = user?.Id
            };

            Database.Respondents.Add(respondent);
            await Database.SaveChangesAsync();

            foreach (var item in model)
            {
                var answer = new Answer
                {
                    OptionId = item.OptionId,
                    QuestionId = item.QuestionId,
                    Content = item.Content,
                    RespondentId = respondent.Id
                };
                Database.Answers.Add(answer);
            }

            await Database.SaveChangesAsync();

            return Status(true);
        }
    }
}
