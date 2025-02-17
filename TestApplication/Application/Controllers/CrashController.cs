using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestApplication.Core.Interfaces.Crash;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace TestApplication.Application.Controllers
{
    [Route("api/crash")]
    [ApiController]
    [Authorize]
    public class CrashController : ControllerBase
    {
        private readonly ICrashService _crashService;
        public CrashController(ICrashService activitiesRepository)
        {
            _crashService = activitiesRepository;
        }
        /// <summary>
        /// Получить запись об аварии по ID
        /// </summary>
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
        /// <summary>
        /// Получить записи обо всех авариях
        /// </summary>
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
        /// <summary>
        /// Получить записи обо всех авариях, созданных указанным пользователем
        /// </summary>
        [HttpGet("by-userId/{id}")]
        public async Task<IActionResult> ByUserId(Guid id)
        {
            var result = await _crashService.GetByUserId(id);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Value);
        }
        /// <summary>
        /// Удалить данные об аварии (операции удаляются каскадно)
        /// </summary>
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

        /// <summary>
        /// Создать новую запись об аварии
        /// </summary>
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

        /// <summary>
        /// Обновить запись об аварии
        /// </summary>
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
