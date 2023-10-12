using GarageDoorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorMonitor.Controllers
{
    [ApiController]
    [Route("voltage")]
    [Authorize]
    public class VoltageController : Controller
    {
        private readonly IVoltageService _service;

        public VoltageController(IVoltageService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Save([FromQuery] double value)
        {
            await _service.SaveAsync(value);
            return Ok();
        }
    }
}
