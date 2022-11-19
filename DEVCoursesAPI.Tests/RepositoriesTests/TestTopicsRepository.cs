using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Tests.RepositoriesTests
{
    public class TestTopicsRepository
    {

        [Fact]
        public async void CreateAsync_ShouldReturnGuidWhenCreatingTopic()
        {
            // Arrange
            ITopicsRepository repo = new TopicsRepository(new TestCoursesDbContextFactory());
            Topic topic = new() { Content = "Content", Name = "Name", Type = "Type" };

            // Act
            Guid result = await repo.CreateAsync(topic);

            // Assert
            Assert.IsType<Guid>(result);
        }
    }
}
