using System.Linq;
using InquirerAPI.Website.Controllers;
using InquirerAPI.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InquirerAPI.API.Controllers
{
    [Authorize]
    [Area("API")]
    [Produces("application/json")]
    [Route("[area]/[controller]")]
    public class KeyTypesController : GenericController
    {
        public KeyTypesController(DatabaseContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor)
        {
        }

        [HttpGet]
        public JsonResult Get()
        {
            var types = Database.KeyType
                .Select(t => Models.KeyType.Generate(t));

            return Json(types);
        }
    }
}
