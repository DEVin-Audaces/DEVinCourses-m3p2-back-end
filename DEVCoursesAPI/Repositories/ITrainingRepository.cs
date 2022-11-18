using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
        public interface ITrainingRepository<TEntity>: IEntity<TEntity>
        {
        public List<TrainingNotRegistered> UserLoginTrainingsList(Guid userId);

        }
}
