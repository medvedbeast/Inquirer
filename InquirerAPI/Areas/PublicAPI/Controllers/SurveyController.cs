using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InquirerAPI.PublicAPI.Data;
using InquirerAPI.PublicAPI.Infrastructure.Filters;
using InquirerAPI.PublicAPI.Models;
using InquirerDLL.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InquirerAPI.PublicAPI.Controllers
{
    [HandleExceptions]
    [AuthorizeViaToken]
    [Area("PublicAPI")]
    [Produces("application/json")]
    [Route("public_api/surveys")]
    public class SurveyController : GenericController
    {
        public SurveyController(DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var survey = Database.Surveys
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if (survey == null)
            {
                return RequestError("Опитування не знайдено.");
            }

            return Result(new InquirerDLL.Entities.Survey
            {
                Id = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                StartDate = survey.StartDate,
                EndDate = survey.EndDate,
                IsOpen = survey.IsOpen,
                IsAuthenticationRequired = survey.IsAuthenticationRequired,
                CreatorId = survey.CreatorId,
                Questions = survey.Questions.OrderBy(q => q.Index).Select(q => new InquirerDLL.Entities.Question
                {
                    Id = q.Id,
                    Title = q.Title,
                    Image = q.Image,
                    TypeId = q.TypeId,
                    Index = q.Index,
                    IsRequired = q.IsRequired,
                    Options = q.Options.OrderBy(o => o.Index).Select(o => new InquirerDLL.Entities.Option
                    {
                        Id = o.Id,
                        Label = o.Label,
                        Value = o.Value,
                        Image = o.Image,
                        IsCustom = o.IsCustom
                    })
                })
            });
        }

        [HttpGet]
        public IActionResult Get(bool? isOpen = null)
        {
            var surveys = Database.Surveys
                .Where(s => s.CreatorId == CurrentUser.Id);

            if (isOpen != null)
            {
                surveys = surveys
                    .Where(s => s.IsOpen == isOpen);
            }

            if (surveys.Count() < 1)
            {
                return RequestError("Опитування не знайдені.");
            }

            return Result(surveys.Select(s => new InquirerDLL.Entities.Survey
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                IsOpen = s.IsOpen,
                IsAuthenticationRequired = s.IsAuthenticationRequired,
                CreatorId = s.CreatorId
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]InquirerDLL.Entities.Survey model)
        {
            var survey = new Survey
            {
                CreatorId = (int)model.CreatorId,
                Title = "Шаблон опитування",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            Database.Surveys.Add(survey);

            await Database.SaveChangesAsync();

            return Result(survey.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]InquirerDLL.Entities.Survey model, int id)
        {
            var survey = Database.Surveys
                .Where(s => s.Id == id)
                .FirstOrDefault();

            survey.Title = model.Title;
            survey.Description = model.Description;
            survey.StartDate = model.StartDate;
            survey.EndDate = model.EndDate;
            survey.IsOpen = (bool)model.IsOpen;
            survey.IsAuthenticationRequired = (bool)model.IsAuthenticationRequired;

            await Database.SaveChangesAsync();

            var newQuestions = new List<Question>();
            var newOptions = new List<Option>();
            if (model.Questions.Count() > 0)
            {
                int questionIndex = 0;
                foreach (var i in model.Questions)
                {
                    Question question = null;
                    if (i.Id == null)
                    {
                        question = new Question
                        {
                            Title = i.Title,
                            TypeId = (int)i.TypeId,
                            SurveyId = survey.Id,
                            Image = i.Image,
                            Index = questionIndex,
                            IsRequired = (bool)i.IsRequired
                        };
                        Database.Questions.Add(question);
                    }
                    else
                    {
                        question = Database.Questions
                            .Where(q => q.Id == i.Id)
                            .FirstOrDefault();

                        question.Title = i.Title;
                        question.TypeId = (int)i.TypeId;
                        question.Image = i.Image;
                        question.Index = questionIndex;
                        question.IsRequired = (bool)i.IsRequired;
                    }
                    questionIndex++;

                    newQuestions.Add(question);
                }

                await Database.SaveChangesAsync();

                int index = 0;
                foreach (var i in model.Questions)
                {
                    if (i.Options.Count() > 0)
                    {
                        int optionIndex = 0;
                        foreach (var i2 in i.Options)
                        {
                            Option option = null;
                            if (i2.Id == null)
                            {
                                option = new Option
                                {
                                    Label = i2.Label,
                                    Value = i2.Value,
                                    Image = i2.Image,
                                    IsCustom = (bool)i2.IsCustom,
                                    QuestionId = newQuestions[index].Id,
                                    Index = optionIndex
                                };
                                Database.Options.Add(option);
                            }
                            else
                            {
                                option = Database.Options
                                    .Where(o => o.Id == i2.Id)
                                    .FirstOrDefault();

                                option.Label = i2.Label;
                                option.Value = i2.Value;
                                option.Image = i2.Image;
                                option.IsCustom = (bool)i2.IsCustom;
                                option.Index = optionIndex;
                            }
                            optionIndex++;

                            newOptions.Add(option);
                        }
                    }
                    index++;
                }

                await Database.SaveChangesAsync();
            }

            var oldQuestions = Database.Questions
                .Where(o => o.SurveyId == survey.Id);

            oldQuestions = oldQuestions.Except(newQuestions);
            Database.Questions.RemoveRange(oldQuestions);

            var oldOptions = Database.Options
                .Include(o => o.Question)
                .Where(o => o.Question.SurveyId == survey.Id);

            oldOptions = oldOptions.Except(newOptions);
            Database.Options.RemoveRange(oldOptions);

            await Database.SaveChangesAsync();

            return Status(true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var survey = Database.Surveys
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if (survey == null)
            {
                return RequestError("Опитування не знайдене.");
            }

            Database.Surveys.Remove(survey);

            await Database.SaveChangesAsync();

            return Status(true);
        }

        [HttpGet("{id}/[action]")]
        public IActionResult Statistics(int id)
        {
            var survey = Database.Surveys
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Options)
                        .ThenInclude(o => o.Answers)
                .Include(s => s.Questions)
                    .ThenInclude(q => q.Answers)
                .Where(s => s.Id == id)
                .FirstOrDefault();

            if (survey == null)
            {
                return RequestError("Опитування не знайдено.");
            }

            var statistics = new InquirerDLL.Output.Survey.Statistics
            {
                Questions = new List<InquirerDLL.Output.Survey.Statistics.Question>()
            };

            if (survey.Questions.Count > 0)
            {
                var questions = survey.Questions
                    .OrderBy(q => q.Index);

                foreach (var q in questions)
                {
                    var question = new InquirerDLL.Output.Survey.Statistics.Question
                    {
                        Id = q.Id,
                        Title = q.Title,
                        TypeId = q.TypeId,
                        Options = new List<InquirerDLL.Output.Survey.Statistics.Option>()
                    };

                    if (q.Options.Count > 0 && q.TypeId != (int)QUESTION_TYPE.RANGE)
                    {
                        var options = q.Options
                            .OrderBy(o => o.Index);

                        foreach (var o in options)
                        {
                            var option = new InquirerDLL.Output.Survey.Statistics.Option
                            {
                                Id = o.Id,
                                Label = o.Label,
                                Value = o.Value,
                                Answers = new List<InquirerDLL.Output.Survey.Statistics.Answer>()
                            };

                            if (o.Answers.Count > 0)
                            {
                                var answers = o.Answers
                                    .Select(a => a.Content)
                                    .Distinct();

                                foreach (var a in answers)
                                {
                                    var answer = new InquirerDLL.Output.Survey.Statistics.Answer
                                    {
                                        Content = a,
                                        Quantity = o.Answers
                                            .Where(_ => _.Content == a)
                                            .Count()
                                    };

                                    option.Answers.Add(answer);
                                }
                            }

                            option.Quantity = option.Answers
                                .Sum(_ => _.Quantity);
                            question.Options.Add(option);
                        }


                    }
                    else if (q.Answers.Count > 0)
                    {
                        var answers = q.Answers
                                    .Select(a => a.Content)
                                    .Distinct();

                        foreach (var a in answers)
                        {
                            var option = new InquirerDLL.Output.Survey.Statistics.Option
                            {
                                Id = null,
                                Label = a,
                                Value = null,
                                Answers = new List<InquirerDLL.Output.Survey.Statistics.Answer>()
                                {
                                    new InquirerDLL.Output.Survey.Statistics.Answer
                                    {
                                        Content = null,
                                        Quantity = q.Answers
                                            .Where(_ => _.Content == a)
                                            .Count()
                                    }
                                }
                            };

                            option.Quantity = option.Answers
                                .Sum(_ => _.Quantity);
                            question.Options.Add(option);
                        }
                    }

                    question.Maximum = question.Options
                            .Max(_ => _.Quantity);
                    statistics.Questions.Add(question);
                }
            }

            return Result(statistics);
        }
    }
}
