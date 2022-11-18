using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.DTOs;
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

        public async Task<bool> DeleteRegistration(Guid userID, Guid trainingID, Guid[] topicsID)
        {
            TrainingUser? registration =
                await _context.TrainingUsers.FirstOrDefaultAsync(x => x.UserId == userID && x.TrainingId == trainingID);
            if (registration is not null) _context.TrainingUsers.Remove(registration);
            foreach (var topicID in topicsID)
            {
                TopicUser? topicUser = await _context.TopicUsers.FirstOrDefaultAsync(x => x.UserId == userID && x.TopicId == topicID);
                if (topicUser is not null) _context.TopicUsers.Remove(topicUser);
            }
            return await _context.SaveChangesAsync() > 0;
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

        public List<TrainingNotRegistered> UserLoginTrainingsList(Guid userId)
        {

            var trainings = _context.Trainings.ToList();
            var trainingsUsers = _context.TrainingUsers.Where(x => x.UserId == userId).ToList();

            List<TrainingNotRegistered> FilteredList = new List<TrainingNotRegistered>();

            foreach (var training in trainings)
            {
                foreach (var trainingUser in trainingsUsers)
                {
                    if (training.Id == trainingUser.TrainingId)
                    {
                        var newTraining = new TrainingNotRegistered()
                        {
                            Id = training.Id,
                            Name = training.Name,
                            Summary = training.Summary,
                            Duration = training.Duration,
                            Instructor = training.Instructor,
                            Author = training.Author,
                            Active = training.Active
                        };

                        FilteredList.Add(newTraining);
                    } 
                }
            }

            trainings.RemoveAll(x => trainingsUsers.Any(y => x.Id == y.TrainingId));
            trainings.ForEach(training => 
            {
                var newTraining = new TrainingNotRegistered()
                {
                    Id = training.Id,
                    Name = training.Name,
                    Summary = training.Summary,
                    Duration = training.Duration,
                };
                FilteredList.Add(newTraining);
            });

            return FilteredList;
        }
        public async Task UpdateTrainingUser(TrainingUser trainingUser)
        {
            _context.Entry(trainingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

