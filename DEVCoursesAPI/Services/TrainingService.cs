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
            List<TopicUser> topicList = await _repository.GetTopicUsers(trainingUser);
            var topicNotCompleted = topicList.FirstOrDefault(topic => !topic.Completed);
            if (topicNotCompleted is not null) return false;

            trainingUser.Completed = true;
            await _repository.UpdateTrainingUser(trainingUser);
            return true;
        }
    }
}
