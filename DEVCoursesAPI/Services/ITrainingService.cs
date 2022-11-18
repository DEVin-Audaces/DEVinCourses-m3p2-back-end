using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Services
{
    public interface ITrainingService
    {
        public List<TrainingNotRegistered> UserLoginTrainingsList(Guid userId);
        Task<bool> CompleteTraining(TrainingUser trainingUser);
        Task <bool> DeleteRegistration(Guid userID, Guid trainingID, Guid[] topicsID);
        Task<ReadTrainingDto?> GetByIdAsync(Guid id);
        Task<bool> CreateTrainingRegistrationAsync(TrainingRegistrationDto dto);
    }
}
