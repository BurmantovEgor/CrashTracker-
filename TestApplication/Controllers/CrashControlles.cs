using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;
using TestApplication.Interfaces.Crash;

namespace TestApplication.Controllers
{
    [Route("api/crash")]
    [ApiController]
    public class CrashControlles : ControllerBase
    {
        private readonly ICrashService _crashService;
        public CrashControlles(ICrashService activitiesRepository)
        {
            _crashService = activitiesRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _crashService.GetById(id);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _crashService.GetAll();
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
           var result = await _crashService.Remove(id);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CrashDTOCreate activityDTO)
        {
            var result = await _crashService.Create(activityDTO);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CrashDTOUpdate activityDTO)
        {

           var result = await _crashService.Update(activityDTO);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }
    }
}
