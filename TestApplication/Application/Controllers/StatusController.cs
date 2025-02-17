using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Core.Interfaces.Status;

namespace TestApplication.Application.Controllers
{

    [Route("api/status")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }
        /// <summary>
        /// Получить список всех статусов 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusService.GetAll();
            return Ok(statuses);
        }

    }
}
