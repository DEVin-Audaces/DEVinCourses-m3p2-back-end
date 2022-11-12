using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Services
{
    public interface ITrainingService
    {
        Task<bool> CompleteTraining(TrainingUser trainingUser);
    }
}
