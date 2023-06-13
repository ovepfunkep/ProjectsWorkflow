using AutoMapper;
using DataAccess.Models;
using BusinessLogic.DTOs;

namespace WebAPI.MappingProfiles
{
    /// <summary>
    /// Класс для маппирования должностей
    /// </summary>
    public class PositionMappingProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PositionMappingProfile()
        {
            CreateMap<Position, PositionDTO>();
            CreateMap<PositionDTO, Position>();
        }
    }
}
