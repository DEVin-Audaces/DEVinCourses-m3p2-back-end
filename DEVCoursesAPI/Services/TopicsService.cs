using DEVCoursesAPI.Data.DTOs.TopicDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    public class TopicsService : ITopicsService
    {
        private readonly ITopicsRepository _topicsRepository;

        public TopicsService(ITopicsRepository topicsRepository)
        {
            _topicsRepository = topicsRepository;
        }

        public async Task CreateTopicsAsync(CreateTopicDto topicDto, Guid moduleId)
        {
            Topic topic = (Topic)topicDto;
            topic.ModuleId = moduleId;

            await _topicsRepository.CreateAsync(topic);
        }
    }
}
