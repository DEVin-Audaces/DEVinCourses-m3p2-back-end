using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    public class TrainingService : ITrainingService
    { 
        private readonly ITrainingRepository<Training> _repository;

        public TrainingService(ITrainingRepository<Training> repository)
        {
            _repository = repository;
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
