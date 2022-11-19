using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Tests.RepositoriesTests
{
    public class TestTrainingsRepository
    {
        private readonly Training training;
        private readonly Users user;

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

            user = new Users()
            {
                    Name = "tese",
                    Email = "adsfasdd@gmail.com",
                    Age = 18,
                    CPF = 2555622,
                    Password = "senhra"
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
        public async void GetTopics_ShouldReturnTopicsList()
        {
            TrainingRepository repository = new(new TestCoursesDbContextFactory());

            Guid trainingId = await repository.CreateTraining(training);

            var listTopics = await repository.GetTopics(trainingId);

            Assert.IsType<List<Topic>>(listTopics);
        }

        [Fact]
        public async void GetFilteredTopicUsers()
        {
            TrainingRepository repository = new(new TestCoursesDbContextFactory());
            UsersRepository userRepository = new(new TestCoursesDbContextFactory());

            Guid trainingId = await repository.CreateTraining(training);
            Guid userId = userRepository.Add(user);

            var topics = await repository.GetTopics(trainingId);

            var listTopics = await repository.GetFilteredTopicUsers(topics, userId);

            Assert.IsType<List<TopicUser>>(listTopics);
        }
    }
}
