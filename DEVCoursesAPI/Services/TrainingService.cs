using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    
        public class TrainingService : ITrainingService
        {
            private readonly ITrainingRepository<Training> _trainingRepository;

            public TrainingService(ITrainingRepository<Training> trainingRepository)
            {
                _trainingRepository = trainingRepository;
            }


            public List<TrainingNotRegistered> UserLoginTrainingsList(Guid userId)
            {
                return _trainingRepository.UserLoginTrainingsList(userId);
            }
        
        }

    
}
