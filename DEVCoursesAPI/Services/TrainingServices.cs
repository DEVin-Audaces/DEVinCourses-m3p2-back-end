using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    public class TrainingServices : ITrainingService
    {
        private readonly ITrainingRepository<Training> _trainingRepository;

        public TrainingServices(ITrainingRepository<Training> trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }


        public bool DeleteRegistration(string userID, string trainingID, string[] topicsID)
        {
            return _trainingRepository.DeleteRegistration( userID, trainingID, topicsID);
        }
    }
}
