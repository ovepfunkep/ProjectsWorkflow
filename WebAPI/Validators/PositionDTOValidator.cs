using BusinessLogic.DTOs;
using FluentValidation;
using BusinessLogic.Services.Interfaces;

namespace WebAPI.Validators
{
    /// <summary>
    /// Класс для валидации DTO-должностей
    /// </summary>
    public class PositionDTOValidator : AbstractValidator<PositionDTO>
    {
        private readonly IPositionService _positionService;

        /// <summary>
        /// Конструктор
        /// </summary>
        public PositionDTOValidator(IPositionService positionService)
        {
            _positionService = positionService;

            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Имя не может быть пустым.");
            RuleFor(p => p.Name)
                .Must(NotBeDuplicateName)
                .WithMessage("Имя и Id соответствуют разным объектам.");
        }

        private bool NotBeDuplicateName(PositionDTO positionDTO, string name)
        {
            var position = _positionService.GetById(positionDTO.Id ?? 0);
            return position == null || position.Name == name;
        }
    }

}
