using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Validators;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Контроллер для управлениями сотрудниками
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <returns></returns>
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Получить всех сотрудников
        /// </summary>
        /// <returns>Список всех DTO-сотрудников</returns>
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            try
            {
                var employees = _employeeService.GetAll();
                return Ok(employees);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Получить сотрудника по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>DTO-сотрудник</returns>
        [HttpGet("{id}")]
        public ActionResult<EmployeeDTO> GetEmployeeById(int id)
        {
            try
            {
                var employee = _employeeService.GetById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <param name="EmployeeDTO">DTO-сотрудник для создания</param>
        /// <returns>Созданный DTO-сотрудник</returns>
        [HttpPost]
        public ActionResult<EmployeeDTO> CreateEmployee(EmployeeDTO EmployeeDTO)
        {
            try
            {
                var validator = new EmployeeDTOValidator();
                var validationResult = validator.Validate(EmployeeDTO);
                if (validationResult.IsValid)
                {
                    var createdEmployee = _employeeService.Add(EmployeeDTO);
                    return Ok(createdEmployee);
                }
                else
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Обновить сотрудника по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <param name="EmployeeDTO">DTO-сотрудник для обновления</param>
        /// <returns>Обновленный DTO-сотрудник</returns>
        [HttpPut]
        public IActionResult UpdateEmployee(EmployeeDTO EmployeeDTO)
        {
            try
            {
                var validator = new EmployeeDTOValidator();
                var validationResult = validator.Validate(EmployeeDTO);

                if (validationResult.IsValid)
                {
                    var updatedEmployee = _employeeService.Update(EmployeeDTO);
                    if (updatedEmployee == null)
                    {
                        return NotFound();
                    }

                    return Ok(updatedEmployee);
                }
                else
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        /// <summary>
        /// Удалить сотрудника по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>Результат операции удаления</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var deletedEmployee = _employeeService.Delete(id);
                if (deletedEmployee == false)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
