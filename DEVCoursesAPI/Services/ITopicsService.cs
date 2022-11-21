using DEVCoursesAPI.Data.DTOs.TopicDTO;

namespace DEVCoursesAPI.Services
{
    public interface ITopicsService
    {
        Task CreateTopicsAsync(CreateTopicDto topicDto, Guid moduleId);
        Task<bool> CompleteTopic(Guid userId, Guid trainingId);
    }
}
