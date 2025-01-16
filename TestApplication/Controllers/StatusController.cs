using Microsoft.AspNetCore.Mvc;
using TestApplication.Interfaces.Status;


namespace TestApplication.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController: ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusService.GetAll();
            return Ok(statuses);
        }
       
    }
}
