using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories {
    public class TopicsRepository : ITopicsRepository {
        private readonly IDbContextFactory<DEVCoursesContext> _dbFactory;

        public TopicsRepository(IDbContextFactory<DEVCoursesContext> dbFactory) {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> CreateAsync(Topic topic) {
            using (var db = _dbFactory.CreateDbContext()) {
                db.Topics.Add(topic);
                await db.SaveChangesAsync();

                return topic.Id;
            }
        }
        public async Task<TopicUser> GetTopicUser(Guid userId, Guid topicId)
        {
            using (var db = _dbFactory.CreateDbContext())
            {
                var topicUser = await db.TopicUsers
                    .Where(t => t.UserId == userId && t.TopicId == topicId).FirstOrDefaultAsync();
                return topicUser;
            }
        }

        public async Task<bool> UpdateTopicUser(TopicUser topicUser)
        {
            using (var db = _dbFactory.CreateDbContext())
            {
                db.Entry(topicUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}