using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
    public interface ITrainingRepository
    {
        Task<bool> DeleteRegistration(Guid userID, Guid trainingID, Guid[] topicsID);
        Task<List<TopicUser>> GetFilteredTopicUsers(List<Topic> topics, Guid userId);
        Task<List<Topic>> GetTopics(Guid trainingId);
        Task<TrainingUser> GetTrainingUser(Guid userId, Guid trainingId);
        Task UpdateTrainingUser(TrainingUser trainingUser);
        Task<bool> CheckForActiveStudents(Guid id);
    }
}
