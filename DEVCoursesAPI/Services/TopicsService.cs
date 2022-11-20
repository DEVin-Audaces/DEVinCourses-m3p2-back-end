using DEVCoursesAPI.Data.DTOs.TopicDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Services
{
    public class TopicsService : ITopicsService
    {
        private readonly ITopicsRepository _topicsRepository;

        public TopicsService(ITopicsRepository topicsRepository)
        {
            _topicsRepository = topicsRepository;
        }

        public async Task<bool> CompleteTopic(Guid userId, Guid trainingId)
        {
            var topicUser = await _topicsRepository.GetTopicUser(userId, trainingId);
            if (topicUser == null) return false;

            topicUser.Completed = true;
            return await _topicsRepository.UpdateTopicUser(topicUser);
        }

        public async Task CreateTopicsAsync(CreateTopicDto topicDto, Guid moduleId)
        {
            Topic topic = (Topic)topicDto;
            topic.ModuleId = moduleId;

            await _topicsRepository.CreateAsync(topic);
        }


    }
}
