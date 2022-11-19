using DEVCoursesAPI.Data.DTOs.ModuleDTO;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVCoursesAPI.Tests.ServicesTests
{
    public class TestTrainingsService
    {
        private readonly Mock<ITrainingRepository> _trainingsRepository;
        private readonly Mock<IModulesService> _modulesService;

        public TestTrainingsService()
        {
            _trainingsRepository = new Mock<ITrainingRepository>();
            _modulesService = new Mock<IModulesService>();
        }

        [Fact]
        public async void CreateTrainingAsync_ShouldCreateTraining()
        {
            // Arrange
            Guid trainingId = Guid.NewGuid();

            _trainingsRepository.Setup(repo => repo.CreateTraining(It.IsAny<Training>()))
                                .ReturnsAsync(trainingId);

            TrainingService service = new(_trainingsRepository.Object, _modulesService.Object);

            CreateTrainingDto trainingDto = new CreateTrainingDto();
            trainingDto.Modules = new List<CreateModuleDto>();

            // Act
            Guid result = await service.CreateTrainingAsync(trainingDto);

            // Assert
            Assert.Equal(trainingId, result);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfTrainings()
        {
            //Arrange
            List<Training> list = new() { new Training(), new Training() };

            _trainingsRepository.Setup(repo => repo.GetAll())
                                .ReturnsAsync(list);

            TrainingService service = new(_trainingsRepository.Object, _modulesService.Object);

            // Act
            List<Training> result = await service.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnTrainingWhenValid()
        {
            // Arrange
            Guid trainingId = Guid.NewGuid();
            Training training = new()
            {
                Id = trainingId,
                Author = Guid.NewGuid(),
                Instructor = "Instructor",
                Modules = new List<Module>(),
                Name = "Name",
                Summary = "Summary"
            };

            _trainingsRepository.Setup(repo => repo.GetByIdAsync(trainingId))
                .ReturnsAsync(training);

            TrainingService service = new(_trainingsRepository.Object, _modulesService.Object);

            // Act
            ReadTrainingDto? trainingDto = await service.GetByIdAsync(trainingId);

            // Assert
            Assert.Equal(trainingId, trainingDto.Id);
        }
    }
}
