using BusinessLogic.DTOs;
using FluentValidation;

namespace WebAPI.Validators
{
    /// <summary>
    /// Класс для валидации DTO-проектов
    /// </summary>
    public class ProjectDTOValidator : AbstractValidator<ProjectDTO>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ProjectDTOValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Некорректное название.");
            RuleFor(p => p.CustomerCompany).NotEmpty().WithMessage("Некорректное название заказчика.");
            RuleFor(p => p.ExecutorCompany).NotEmpty().WithMessage("Некорректное название исполнителя.");
            RuleFor(p => p.ProjectManager).Must(pm => pm == null || pm.Id > 0).WithMessage("Неверный идентификатор руководителя.");
            RuleFor(p => p.Employees).Must(employees => employees.All(emp => emp.Id > 0)).WithMessage("Неверные идентификаторы сотрудников.");
        }
    }
}
