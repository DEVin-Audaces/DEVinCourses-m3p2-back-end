using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly IDbContextFactory<DEVCoursesContext> _dbContextFactory;

        public TrainingRepository(IDbContextFactory<DEVCoursesContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<bool> CheckForActiveStudents(Guid id)
        {
            using (var _context = _dbContextFactory.CreateDbContext())
            {
                List<TrainingUser> trainingUsers = await _context.TrainingUsers
                   .Where(trainingusers => trainingusers.TrainingId == id)
                   .ToListAsync();

                int activeStudents = trainingUsers.Where(tu => tu.Completed == false).Count();

                return activeStudents == 0;
            }
        }

        public async Task<bool> DeleteRegistration(Guid userID, Guid trainingID, Guid[] topicsID)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                TrainingUser? registration =
                    await context.TrainingUsers.FirstOrDefaultAsync(x => x.UserId == userID && x.TrainingId == trainingID);
                if (registration is not null) context.TrainingUsers.Remove(registration);
                foreach (var topicID in topicsID)
                {
                    TopicUser? topicUser = await context.TopicUsers.FirstOrDefaultAsync(x => x.UserId == userID && x.TopicId == topicID);
                    if (topicUser is not null) context.TopicUsers.Remove(topicUser);
                }
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<List<TopicUser>> GetFilteredTopicUsers(List<Topic> topics, Guid userId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                List<TopicUser> filteredTopics = new List<TopicUser>();
                await context.TopicUsers.ForEachAsync(topicUser =>
                {
                    topics.ForEach(topic =>
                    {
                        if (topic.Id == topicUser.TopicId && userId == topicUser.UserId)
                            filteredTopics.Add(topicUser);
                    });
                });

                return filteredTopics;
            }
        }

        public async Task<List<Topic>> GetTopics(Guid trainingId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                List<Module> modules = await context.Modules.Where(m => m.TrainingId == trainingId).ToListAsync();

                List<Topic> topics = new List<Topic>();
                await context.Topics.ForEachAsync(topic =>
                {
                    modules.ForEach(m =>
                    {
                        if (topic.ModuleId == m.Id)
                        {
                            topics.Add(topic);
                            return;
                        }
                    });
                });

                return topics;
            }
        }

        public async Task<TrainingUser> GetTrainingUser(Guid userId, Guid trainingId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var trainingUser = await context.TrainingUsers
                .Where(training => training.UserId == userId && training.TrainingId == trainingId)
                .FirstOrDefaultAsync();
                return trainingUser;
            }
        }

        public async Task UpdateTrainingUser(TrainingUser trainingUser)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(trainingUser).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}
