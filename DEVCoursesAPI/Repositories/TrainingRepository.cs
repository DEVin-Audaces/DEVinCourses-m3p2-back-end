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

        public async Task<bool> CompleteTraining(TrainingUser trainingUser)
        {
            List<TopicUser> topicList = _context.TopicUsers
                .Where(x => x.TrainingId == trainingUser.TrainingId).ToList(); 

            if (topicList.FirstOrDefault(topic => !topic.Completed) != null) return false;

            trainingUser.Completed = true;
            _context.Entry(trainingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
                
            return true;
        }

        public IList<Training> GetAll()
        {
            throw new NotImplementedException();
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
    }
}
