using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations
{
    public class EmployeeService : CRUDService<Employee, EmployeeDTO>, IEmployeeService
    {
        //private readonly IPositionRepository _positionRepository;
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper) : base(employeeRepository, mapper)
        {
            //_employeeRepository = employeeRepository;
            //_positionRepository = positionRepository;
            //_mapper = mapper;
        }
    }
}