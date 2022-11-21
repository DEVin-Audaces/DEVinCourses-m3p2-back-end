using DEVCoursesAPI.Data.DTOs.TopicDTO;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Services;
using Moq;

namespace DEVCoursesAPI.Tests.ServicesTests
{
    public class TestTopicServices
    {
        [Fact]
        public void CreateTopicsAsync_ShouldCreateATopic()
        {
            // Arrange
            Mock<ITopicsRepository> mock = new();
            TopicsService service = new(mock.Object);

            Guid moduleId = Guid.NewGuid();
            CreateTopicDto topicDto = new() { Content = ""};

            // Act
            Task result = service.CreateTopicsAsync(topicDto, moduleId);

            // Assert
            Assert.True(result.IsCompleted);
        }

    }
}
