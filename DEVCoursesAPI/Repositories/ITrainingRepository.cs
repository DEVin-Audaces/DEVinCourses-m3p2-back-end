using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
    public interface ITrainingRepository<TEntity>: IEntity<TEntity>
    {
        Task<bool> CompleteTraining(TrainingUser trainingUser);
        Task<TrainingUser> GetTrainingUser(Guid userId, Guid trainingId);    
    }
}
