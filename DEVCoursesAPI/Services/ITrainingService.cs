using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Services
{
    public interface ITrainingService
    {
        Task<bool> CompleteTraining(TrainingUser trainingUser);
        public bool DeleteRegistration(string userID, string trainingID, string[] topicsID);
    }
}
