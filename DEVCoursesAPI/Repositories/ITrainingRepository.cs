using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
    public interface ITrainingRepository<TEntity>: IEntity<TEntity>
    {
        Task<TrainingUser> GetTrainingUser(Guid userId, Guid trainingId);    
        Task<List<TopicUser>> GetTopicUsers(TrainingUser trainingUser);
        Task UpdateTrainingUser(TrainingUser trainingUser);
    }
}
