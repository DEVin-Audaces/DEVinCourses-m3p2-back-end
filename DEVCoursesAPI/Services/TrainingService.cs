using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    public class TrainingService : ITrainingService
    { 
        private readonly ITrainingRepository _repository;

        public TrainingService(ITrainingRepository repository)
        {
            _repository = repository;
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
    }
}
