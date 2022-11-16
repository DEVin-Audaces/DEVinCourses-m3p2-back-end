using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories
{
    public class TrainingRepository : ITrainingRepository<Training>
    {
        private readonly IDbContextFactory<DEVCoursesContext> _dbContextFactory;

        public TrainingRepository(IDbContextFactory<DEVCoursesContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Guid Add(Training model)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRegistration(string userID, string trainingID, string[] topicsID)
        {
          using(var context = _dbContextFactory.CreateDbContext())
            {
                TrainingUser? registration =
                    context.TrainingUsers.FirstOrDefault(x => x.UserId.ToString() == userID && x.TrainingId.ToString() == trainingID);

                
                context.TrainingUsers.Remove(registration);

                

                foreach (var topicID in topicsID)
                {
                    TopicUser? topicUser = context.TopicUsers.FirstOrDefault(x => x.UserId.ToString() == userID && x.TopicId.ToString() == topicID);
                    context.TopicUsers.Remove(topicUser);
                }
                
               
                return context.SaveChanges() > 0;
            }
        }

        public bool Delete(Training model)
        {
            throw new NotImplementedException();
        }

        public bool Update(Training model)
        {
            throw new NotImplementedException();
        }
    }
}
