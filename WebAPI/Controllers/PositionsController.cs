using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Validators;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Контроллер для управлениями должностями
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <returns></returns>
        public PositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        /// <summary>
        /// Получить все должности
        /// </summary>
        /// <returns>Список всех DTO-должностей</returns>
        [HttpGet]
        public ActionResult<IEnumerable<PositionDTO>> GetAllPositions()
        {
            try
            {
                var positions = _positionService.GetAll();
                return Ok(positions);
            }
            catch (Exception e) { return Problem(e.Message); }
        }

        /// <summary>
        /// Получить должность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор должностт</param>
        /// <returns>DTO-должность</returns>
        [HttpGet("{id}")]
        public ActionResult<PositionDTO> GetPositionById(int id)
        {
            try
            {
                var position = _positionService.GetById(id);
                if (position == null)
                {
                    return NotFound();
                }
                return Ok(position);
            }
            catch (Exception e) { return Problem(e.Message); }
        }

        /// <summary>
        /// Создать новую должность
        /// </summary>
        /// <param name="PositionDTO">DTO-должность для создания</param>
        /// <returns>Созданная DTO-должность</returns>
        [HttpPost]
        public ActionResult<PositionDTO> CreatePosition(PositionDTO PositionDTO)
        {
            try
            {
                var validator = new PositionDTOValidator(_positionService);
                var validationResult = validator.Validate(PositionDTO);

                if (validationResult.IsValid)
                {
                    var createdPosition = _positionService.Add(PositionDTO);
                    return Ok(createdPosition);
                }
                else
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception e) { return Problem(e.Message); }
        }

        /// <summary>
        /// Обновить должность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор должности</param>
        /// <param name="PositionDTO">DTO-должность для обновления</param>
        /// <returns>Обновленная DTO-должность</returns>
        [HttpPut]
        public IActionResult UpdatePosition(PositionDTO PositionDTO)
        {
            try
            {
                var updatedPosition = _positionService.Update(PositionDTO);
                if (updatedPosition == null)
                {
                    return NotFound();
                }

                return Ok(updatedPosition);
            }
            catch (Exception e) { return Problem(e.Message); }
        }

        /// <summary>
        /// Удалить должность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор должности</param>
        /// <returns>Результат операции удаления</returns>
        [HttpDelete("{id}")]
        public IActionResult DeletePosition(int id)
        {
            try
            {
                var deletedPosition = _positionService.Delete(id);
                if (deletedPosition == false)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception e) { return Problem(e.Message); }
        }
    }
}
