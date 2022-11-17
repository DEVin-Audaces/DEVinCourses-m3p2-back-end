using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories
{
    public class TrainingRepository : ITrainingRepository<Training>
    {
        private readonly DEVCoursesContext _context;

        public TrainingRepository(DEVCoursesContext context)
        {
            _context = context;
        }

        public Guid Add(Training model)
        {
            throw new NotImplementedException();
        }

        public IList<Training> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool DeleteRegistration(string userID, string trainingID, string[] topicsID)
        {
            TrainingUser? registration =
                _context.TrainingUsers.FirstOrDefault(x => x.UserId.ToString() == userID && x.TrainingId.ToString() == trainingID);

            _context.TrainingUsers.Remove(registration);

            foreach (var topicID in topicsID)
            {
                TopicUser? topicUser = _context.TopicUsers.FirstOrDefault(x => x.UserId.ToString() == userID && x.TopicId.ToString() == topicID);
                _context.TopicUsers.Remove(topicUser);
            }

            return _context.SaveChanges() > 0;
        }

        public async Task<List<TopicUser>> GetFilteredTopicUsers(List<Topic> topics, Guid userId)
        {
            List<TopicUser> filteredTopics = new List<TopicUser>();
                await _context.TopicUsers.ForEachAsync(topicUser =>
                {    
                    topics.ForEach(topic =>
                    {
                        if (topic.Id == topicUser.TopicId && userId == topicUser.UserId)
                            filteredTopics.Add(topicUser);
                    });
                });

            return filteredTopics;
        }

        public async Task<List<Topic>> GetTopics(Guid trainingId)
        {
            List<Module> modules = await _context.Modules.Where(m => m.TrainingId == trainingId).ToListAsync();

            List<Topic> topics = new List<Topic>();
            await _context.Topics.ForEachAsync(topic =>
            {
                modules.ForEach(m =>
                {
                    if (topic.ModuleId == m.Id)
                    {
                        topics.Add(topic);
                        return;
                    }
                } );
            });

            return topics;
        }
        
        public async Task<TrainingUser> GetTrainingUser(Guid userId, Guid trainingId)
        {
            var trainingUser = await _context.TrainingUsers
                .Where(training => training.UserId == userId && training.TrainingId == trainingId)
                .FirstOrDefaultAsync();
            return trainingUser;
        }

        public bool Update(Training model)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTrainingUser(TrainingUser trainingUser)
        {
            _context.Entry(trainingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
