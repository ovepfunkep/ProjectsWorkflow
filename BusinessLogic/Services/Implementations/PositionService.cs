using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using AutoMapper;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations
{
    public class PositionService : CRUDService<Position, PositionDTO>, IPositionService
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public PositionService(IPositionRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public PositionDTO GetByName(string name)
        {
            var position = _repository.GetByName(name);
            var positionDto = _mapper.Map<PositionDTO>(position);
            return positionDto;
        }
    }
}