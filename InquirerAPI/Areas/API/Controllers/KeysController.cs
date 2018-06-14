using InquirerAPI.Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Http;
using InquirerAPI.Website.Data;
using System.Threading.Tasks;
using InquirerAPI.Website.Controllers;
using Microsoft.EntityFrameworkCore;

namespace InquirerAPI.API.Controllers
{
    [Authorize]
    [Area("API")]
    [Produces("application/json")]
    [Route("[area]/[controller]")]
    public class KeysController : GenericController
    {
        public KeysController(DatabaseContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor)
        {
        }


        [HttpGet]
        public JsonResult Get()
        {
            var keys = Database.Key
                .Include(k => k.Type)
                .Where(k => k.UserId == CurrentUser.Id)
                .OrderBy(k => k.Id)
                .Select(k => Models.Key.Generate(k));

            return Json(keys);
        }

        [HttpGet("{id:int}")]
        public JsonResult Get(int id)
        {
            var key = Database.Key
                .Include(k => k.Type)
                .Where(k => k.Id == id)
                .Select(k => Models.Key.Generate(k))
                .FirstOrDefault();

            return Json(key);
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody]Models.Key model)
        {
            Key key = new Key
            {
                UserId = model.UserId,
                TypeId = model.Type.Id,
                Name = model.Name,
                Content = ""
            };

            Database.Key.Add(key);

            await Database.SaveChangesAsync();

            key.Content = Hash(key.Id);
            await Database.SaveChangesAsync();


            return Get(key.Id);
        }

        [HttpPut("{id}")]
        public async Task<JsonResult> Put([FromBody]Models.Key model, int id)
        {
            var key = Database.Key
                .Include(k => k.Type)
                .Where(k => k.Id == id)
                .FirstOrDefault();

            key.Name = model.Name;
            key.TypeId = model.Type.Id;

            await Database.SaveChangesAsync();

            return Get(key.Id);
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var key = Database.Key
                .Where(k => k.Id == id)
                .FirstOrDefault();

            Database.Key.Remove(key);
            await Database.SaveChangesAsync();

            return Json(true);
        }
    }
}
