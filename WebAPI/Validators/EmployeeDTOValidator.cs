using BusinessLogic.DTOs;
using FluentValidation;
using BusinessLogic.Services.Interfaces;

namespace WebAPI.Validators
{
    /// <summary>
    /// Класс для валидации DTO-сотрудников
    /// </summary>
    public class EmployeeDTOValidator : AbstractValidator<EmployeeDTO>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EmployeeDTOValidator()
        {
            RuleFor(e => e.Name).NotEmpty().WithMessage("Некорректное имя.");
            RuleFor(e => e.Position)
                .NotEmpty()
                .Must(p => p!.Id != 0 || p.Name != null)
                .WithMessage("Некорректная должность.");      
        }
    }
}
