using Microsoft.EntityFrameworkCore;
using BusinessLogic.Services.Interfaces;
using AutoMapper;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations
{
    public class CRUDService<T, TDTO> : ICRUDService<T,TDTO> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;

        public CRUDService(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual IEnumerable<TDTO> GetAll()
        {
            var entities = _repository.GetAll();
            var dtos = _mapper.Map<IEnumerable<TDTO>>(entities);
            return dtos;
        }

        public TDTO GetById(int id)
        {
            var entity = _repository.GetById(id);
            var dto = _mapper.Map<TDTO>(entity);
            return dto;
        }

        public virtual TDTO Add(TDTO dto)
        {
            var entity = _mapper.Map<T>(dto);
            entity = _repository.Add(entity);
            return _mapper.Map<TDTO>(entity);
        }

        public TDTO Update(TDTO dto)
        {
            var entity = _mapper.Map<T>(dto);
            entity = _repository.Update(entity);
            return _mapper.Map<TDTO>(entity);
        }

        public bool Delete(int id)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
                return false;

            _repository.Delete(id);
            return true;
        }
    }
}