using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
    public interface ITrainingRepository<IEntity> :IEntity<IEntity>
    {
        
        public bool DeleteRegistration(string userID, string trainingID, string[] topicsID);
    }
}
