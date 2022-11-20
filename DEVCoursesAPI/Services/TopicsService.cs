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

            if (topic.Content.Contains("watch?v="))
                topic.Content = topic.Content.Replace("watch?v=", "embed/");

            if (topic.Content.Contains("&"))
            {
                int index = topic.Content.IndexOf("&");
                topic.Content = topic.Content.Substring(0, index);
            }

            await _topicsRepository.CreateAsync(topic);
        }


    }
}