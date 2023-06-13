using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Validators;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Контроллер для управлениями проектами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <returns></returns>
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Получить все проекты
        /// </summary>
        /// <returns>Список всех DTO-проектов</returns>
        [HttpGet]
        public ActionResult<IEnumerable<ProjectDTO>> GetAllProjects()
        {
            try
            {
                var projects = _projectService.GetAll();
                return Ok(projects);
            }
            catch (Exception e) { return Problem(title: e.Message); }
        }

        /// <summary>
        /// Получить проект по индентификатору
        /// </summary>
        /// <param name="projectId">Проект</param>
        /// <returns>DTO-проект</returns>
        [HttpGet("{id}")]
        public ActionResult<ProjectDTO> GetProjectById(int projectId)
        {
            try
            {
                var project = _projectService.GetById(Convert.ToInt32(projectId));
                if (project == null)
                {
                    return NotFound();
                }
                return Ok(project);
            }
            catch (Exception e) { return Problem(title: e.Message); }
        }

        /// <summary>
        /// Создать новый проект
        /// </summary>
        /// <param name="ProjectDTO">DTO-проект для создания</param>
        /// <returns>Созданный DTO-проект</returns>
        [HttpPost]
        public ActionResult<ProjectDTO> CreateProject(ProjectDTO ProjectDTO)
        {
            try
            {
                ProjectDTO.Id = 0;

                var validator = new ProjectDTOValidator();
                var validationResult = validator.Validate(ProjectDTO);

                if (validationResult.IsValid)
                {
                    var createdProject = _projectService.Add(ProjectDTO);
                    return Ok(createdProject);
                }
                else
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception e) { return Problem(title: e.Message); }
        }

        /// <summary>
        /// Обновить проект 
        /// </summary>
        /// <param name="ProjectDTO">DTO-проект для обновления</param>
        /// <returns>Результат операции обновления</returns>
        [HttpPut]
        public IActionResult UpdateProject(ProjectDTO ProjectDTO)
        {
            try
            {
                var validator = new ProjectDTOValidator();
                var validationResult = validator.Validate(ProjectDTO);

                if (validationResult.IsValid)
                {
                    var updatedProject = _projectService.Update(ProjectDTO);
                    if (updatedProject == null)
                    {
                        return NotFound();
                    }

                    return NoContent();
                }
                else
                {
                    var errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return BadRequest(errors);
                }
            }
            catch (Exception e) { return Problem(title: e.Message); }
        }

        /// <summary>
        /// Удалить проект по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор проекта</param>
        /// <returns>Результат операции удаления</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var deletedProject = _projectService.Delete(id);
                if (deletedProject == false)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception e) { return Problem(title: e.Message); }
        }
    }
}
