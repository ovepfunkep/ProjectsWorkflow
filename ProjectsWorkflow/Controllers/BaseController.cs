using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Services;

namespace WebAPI.Controllers
{
    public class BaseController<TEntity, TDto> : ControllerBase where TEntity : class where TDto : class
    {
        private readonly IBaseService<TEntity, TDto> _service;

        public BaseController(IService<TEntity, TDto> service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TDto>> GetAll()
        {
            var items = _service.GetAll();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public ActionResult<TDto> GetById(int id)
        {
            var item = _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<TDto> Create(TDto dto)
        {
            var createdItem = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var updatedItem = _service.Update(dto);
            if (updatedItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deletedItem = _service.Delete(id);
            if (deletedItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}