using GarageDoorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorMonitor.Controllers
{
    [ApiController]
    [Route("garage-door")]
    [Authorize]
    public class GarageDoorController : Controller
    {
        private readonly IGarageDoorService _service;
        public GarageDoorController(IGarageDoorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var statuses = await _service.GetAsync();
            return Ok(statuses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Status(int id)
        {
            var status = await _service.GetAsync(id.ToString());
            return Ok(status);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Status(int id, [FromQuery] int isOpen)
        {
            await _service.SetAsync(id.ToString(), isOpen);
            return Ok();
        }
    }
}
