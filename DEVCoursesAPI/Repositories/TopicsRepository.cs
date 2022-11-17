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
    }
}