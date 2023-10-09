using GarageDoorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorMonitor.Controllers
{
    [ApiController]
    [Route("config")]
    [Authorize]
    public class ConfigController : Controller
    {
        private readonly IConfigService _service;

        public ConfigController(IConfigService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var config = await _service.GetAsync(name);
            return Ok(config);
        }
    }
}
