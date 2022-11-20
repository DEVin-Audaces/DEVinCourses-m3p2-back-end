using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Tests.RepositoriesTests
{
    public class TestTrainingsRepository
    {
        private readonly Training training;

        public TestTrainingsRepository()
        {
            training = new Training()
            {
                Name = "Name",
                Summary = "Summary",
                Duration = 10,
                Instructor = "Instructor",
                Author = Guid.NewGuid(),
                Active = false
            };
        }
        [Fact]
        public async void CreateTraining_ShouldReturnGuidWhenCreatingTraining()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            // Act
            Guid result = await repository.CreateTraining(training);

            // Assert
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfTrainings()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            await repository.CreateTraining(training);

            // Act
            List<Training> result = await repository.GetAll();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnTrainingWhenValid()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            Guid trainingId = await repository.CreateTraining(training);

            // Act
            Training? result = await repository.GetByIdAsync(trainingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(trainingId, result.Id);
        }

        [Fact]
        public async void GetByIdAsync_ShouldReturnNullWhenInvalid()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            Guid invalidId = Guid.NewGuid();

            // Act
            Training? result = await repository.GetByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void SuspendAsync_ShouldSuspendActiveTraining()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            training.Active = true;
            Guid trainingId = await repository.CreateTraining(training);

            // Act
            bool result = await repository.SuspendAsync(trainingId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void SuspendAsync_ShouldNotSuspendInactiveTraining()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            training.Active = false;
            Guid trainingId = await repository.CreateTraining(training);

            // Act
            bool result = await repository.SuspendAsync(trainingId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void GetUsersRegisteredInTraining_ShouldReturnRegisteredUsers()
        {
            //Arrange
            TestCoursesDbContextFactory dbFactory = new ();
            UsersRepository usersRepository = new(dbFactory);
            TrainingRepository repository = new(dbFactory);

            Users user = new() { Email = "", Name = "", Password = ""};
            Guid userId = usersRepository.Add(user);

            training.Active = true;
            Guid trainingId = await repository.CreateTraining(training);          

            TrainingRegistrationDto registrationDto = new() { TrainingId = trainingId, UserId = userId, TopicIds = new List<Guid>() };
            await repository.CreateTrainingRegistration(registrationDto);

            // Act
            RegisteredUsers registeredUsers = await repository.GetUsersRegisteredInTraining(trainingId);
            int result = registeredUsers.ActiveUsers.Count;

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async void GetReports_ShouldReturnListOfReports()
        {
            // Arrange
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            await repository.CreateTraining(training);

            // Act
            List<TrainingReport> reports = await repository.GetReports();

            // Assert
            Assert.Single(reports);
        }
    }
}
