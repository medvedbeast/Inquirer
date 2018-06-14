using Inquirer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Inquirer.Controllers
{
    public class ApiController : GenericController
    {
        public ApiController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Call([FromBody]ApiCallViewModel model)
        {
            var response = await API.Send(model.Url, new System.Net.Http.HttpMethod(model.Method), model.Data);

            return Json(response);
        }
    }
}
