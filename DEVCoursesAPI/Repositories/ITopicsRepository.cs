
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories {
    public interface ITopicsRepository {
        Task<Guid> CreateAsync(Topic topic);
        Task<TopicUser> GetTopicUser(Guid userId, Guid topicId);
        Task<bool> UpdateTopicUser(TopicUser topicUser);
    }
}