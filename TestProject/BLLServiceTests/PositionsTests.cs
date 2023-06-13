using AutoMapper;
using BusinessLogic.Services.Implementations;
using DataAccess.Repositories.Interfaces;
using NUnit.Framework;
using WebAPI.MappingProfiles;

namespace TestProject
{
    public class PositionsTests
    {
        private PositionService _positionService;

        [SetUp]
        public void Setup()
        {
            var repository = new Mock<IPositionRepository>();
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PositionMappingProfile>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            _positionService = new PositionService(repository.Object, mapper);
        }

        [Test]
        public void Add_ValidEntityOnlyName_ReturnsMappedDTO()
        {
            // Arrange
            var positionDTO = new PositionDTO { Name = "Junior developer"};

            // Act
            var result = _positionService.Add(positionDTO);

            // Assert
            Assert.That(result.Name, Is.EqualTo(positionDTO.Name));
        }
    }
}
