using Microsoft.AspNetCore.Mvc;
using TestApplication.DTO_s;
using TestApplication.Core.Interfaces.Operations;
using Microsoft.AspNetCore.Authorization;

namespace TestApplication.Application.Controllers
{
    [Route("api/operation")]
    [ApiController]
    [Authorize]
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        /// <summary>
        /// Создать операцию для указанной аварии
        /// </summary>
        [HttpPost("{crashId}")]
        public async Task<IActionResult> Create(Guid crashId, [FromForm] string description)
        {
            var result = await _operationService.Create(crashId, description);

            if (result.IsFailure)
            {
                return BadRequest(new { result.Error });
            }

            return Ok();
        }
        /// <summary>
        /// Обновить данные операции
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Update(OperationDTO operationDTO)
        {
            var result = await _operationService.Update(operationDTO);
            if (result.IsFailure)
            {
                return result.Error == "Операция не найдена"
                    ? NotFound(new { result.Error })
                    : BadRequest(new { result.Error });
            }
            return Ok();
        }
        /// <summary>
        /// Получить список операций по ID аварии
        /// </summary>
        [HttpGet("{crashId}")]
        public async Task<IActionResult> GetByCrashId(Guid crashId)
        {
            var result = await _operationService.GetByCrashId(crashId);

            if (result.IsFailure)
            {
                return NotFound(new { result.Error });
            }

            return Ok(result.Value);
        }
        /// <summary>
        /// Удалить данные об операции
        /// </summary>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _operationService.Remove(Id);

            if (result.IsFailure)
            {
                return BadRequest(new { result.Error });
            }

            return Ok();
        }
    }
}
