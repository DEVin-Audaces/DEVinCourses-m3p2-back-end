using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    public class TrainingService : ITrainingService
    { 
        private readonly ITrainingRepository _repository;
        private readonly IModulesService _modulesService;

        public TrainingService(ITrainingRepository repository, IModulesService modulesService)
        {
            _repository = repository;
            _modulesService = modulesService;
        }

        public List<TrainingNotRegistered> UserLoginTrainingsList(Guid userId)
        {
            return _repository.UserLoginTrainingsList(userId);
        }
        public async Task<ReadTrainingDto?> GetByIdAsync(Guid id)
        {
            Training? training = await _repository.GetByIdAsync(id);

            if (training == null)
                return null;

            ReadTrainingDto readTrainingDto = (ReadTrainingDto)training;

            return readTrainingDto;
        }

        public async Task<bool> CompleteTraining(TrainingUser trainingUser)
        {
            var topics = await _repository.GetTopics(trainingUser.TrainingId);
            var filteredListTopicUsers = await _repository.GetFilteredTopicUsers(topics, trainingUser.UserId);

            var topicNotCompleted = filteredListTopicUsers.FirstOrDefault(topic => !topic.Completed);
            if (topicNotCompleted is not null)
                return false;

            trainingUser.Completed = true;
            await _repository.UpdateTrainingUser(trainingUser);
            return true;
        }

        public async Task<bool> DeleteRegistration(Guid userID, Guid trainingID, Guid[] topicsID)
        {
            return await _repository.DeleteRegistration(userID, trainingID, topicsID);
        }

        public async Task<Guid> CreateTrainingAsync(CreateTrainingDto dto)
        {
            Training training = (Training)dto;
            Guid trainingId = await _repository.CreateTraining(training);

            foreach (var module in dto.Modules)
            {
                await _modulesService.CreateModulesAsync(module, trainingId);
            }

            return trainingId;
        }

        public async Task<bool> CreateTrainingRegistrationAsync(TrainingRegistrationDto dto)
        {
            TrainingRegistrationDto training = (TrainingRegistrationDto)dto;
            bool trainingStatus = await _repository.CreateTrainingRegistration(training);

            return trainingStatus;
        }

        public async Task<bool> SuspendAsync(Guid id)
        {
            bool noActiveStudents = await _repository.CheckForActiveStudents(id);

            if (noActiveStudents == false)
                return false;

            return await _repository.SuspendAsync(id);
        }

        public async Task<List<Training>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<RegisteredUsers> GetUsersRegisteredInTraining(Guid trainingId)
        {
            return await _repository.GetUsersRegisteredInTraining(trainingId);
        }

        public async Task<List<TrainingReport>> GetReports()
        {
            List<TrainingReport> reports = await _repository.GetReports();
            reports = reports.OrderByDescending(report => report.TotalFinishedStudents).ToList();

            return reports;
        }
    }
}
