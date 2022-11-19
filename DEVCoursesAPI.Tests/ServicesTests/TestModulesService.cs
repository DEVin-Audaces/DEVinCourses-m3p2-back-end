using DEVCoursesAPI.Data.DTOs.TopicDTO;
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
    public class TestModulesService
    {

        [Fact]
        public void CreateTopicsAsync_ShouldCreateATopic()
        {

            Mock<ITopicsRepository> mock = new();
            TopicsService service = new(mock.Object);

            Guid moduleId = Guid.NewGuid();
            CreateTopicDto topicDto = new();

            Task result = service.CreateTopicsAsync(topicDto, moduleId);

            Assert.True(result.IsCompleted);
        }

    }
}
