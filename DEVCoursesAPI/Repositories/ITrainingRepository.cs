using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
    public interface ITrainingRepository<TEntity>: IEntity<TEntity>
    {
        Task<TrainingUser> GetTrainingUser(Guid userId, Guid trainingId);    
        Task<List<TopicUser>> GetFilteredTopicUsers(List<Topic> topics, Guid userId);
        Task<List<Topic>> GetTopics(Guid trainingId);
        Task UpdateTrainingUser(TrainingUser trainingUser);
        Task<bool> DeleteRegistration(Guid userID, Guid trainingID, Guid[] topicsID);
    }
}
