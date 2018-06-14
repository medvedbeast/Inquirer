using System;
using System.Linq;
using InquirerAPI.Website.Controllers;
using InquirerAPI.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InquirerAPI.API.Controllers
{
    [Authorize]
    [Area("API")]
    [Route("[area]/[controller]")]
    public class ActivitiesController : GenericController
    {
        public ActivitiesController(DatabaseContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(context, configuration, httpContextAccessor)
        {
        }

        [HttpGet]
        public JsonResult Get(int start, int limit)
        {
            var activities = Database.Activity
                .Where(a => a.UserId == CurrentUser.Id)
                .Include(a => a.Key)
                .OrderByDescending(l => l.OccuredOn)
                .Skip(start)
                .Take(limit)
                .Select(a => new
                {
                    a.Id,
                    a.Status,
                    OccuredOn = a.OccuredOn.ToString("HH:mm:ss dd.MM.yyyy"),
                    a.Content,
                    a.ExternalUserId,
                    Key = new Website.Data.Key
                    {
                        Id = a.Key.Id,
                        Name = a.Key.Name,
                        Content = a.Key.Content
                    },
                    a.UserId
                });

            return Json(activities);
        }
    }
}
