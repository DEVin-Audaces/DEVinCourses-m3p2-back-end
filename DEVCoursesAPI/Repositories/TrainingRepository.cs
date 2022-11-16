using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories
{
    public class TrainingRepository : ITrainingRepository<Training>
    {
        private DEVCoursesContext _context;
        
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
            //await _context.Trainings
            //    .Include(training => training.Modules)
            //    .ThenInclude(module => module.Topics)
            //    .FirstOrDefaultAsync(training => training.Id == trainingId);

            _context.Trainings.Where(training => training.Id == trainingId);
            List<Module> modules = await _context.Modules.Where(m => m.TrainingId == trainingId).ToListAsync();

            List<Topic> topics = new List<Topic>();

            modules.ForEach(m => _context.Topics.ForEachAsync(topic =>
            {
                if (topic.ModuleId == m.Id)
                    topics.Add(topic);
            }));

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
