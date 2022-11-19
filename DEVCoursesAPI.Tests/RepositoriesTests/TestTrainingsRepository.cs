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
    }
}
