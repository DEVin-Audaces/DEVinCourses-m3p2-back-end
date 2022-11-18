﻿using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
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

        public Guid Add(Training model)
        {
            throw new NotImplementedException();
        }
        
        public IList<Training> GetAll()
        {
            throw new NotImplementedException();
        }
        public async Task<Training?> GetByIdAsync(Guid id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Trainings
                    .Include(training => training.Modules)
                    .ThenInclude(module => module.Topics)
                    .FirstOrDefaultAsync();
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

        public List<TrainingNotRegistered> UserLoginTrainingsList(Guid userId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                var trainings = context.Trainings.ToList();
                var trainingsUsers = context.TrainingUsers.Where(x => x.UserId == userId).ToList();

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
        }
        public async Task UpdateTrainingUser(TrainingUser trainingUser)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Entry(trainingUser).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
        public async Task<Guid> CreateTraining(Training training)
        {
            using (var db = _dbContextFactory.CreateDbContext()) 
            {
                db.Trainings.Add(training);
                await db.SaveChangesAsync();

                return training.Id;
            }
        }

        public async Task<bool> SuspendAsync(Guid id)
        {
            using (var db = _dbContextFactory.CreateDbContext()) 
             {
                Training? training = await db.Trainings.FirstOrDefaultAsync(training => training.Id == id);

                if (training == null | training.Active == false)
                    return false;

                training.Active = false;
                db.Trainings.Update(training);
                int result = await db.SaveChangesAsync();

                return result > 0;
            }
        }

        public async Task<bool> CheckForActiveStudents(Guid id)
        {
            using (var db = _dbContextFactory.CreateDbContext())
            {
                List<TrainingUser> trainingUsers = await db.TrainingUsers.Where(trainingusers => trainingusers.TrainingId == id).ToListAsync();
                int activeStudents = trainingUsers.Where(tu => tu.Completed == false).Count();

                return activeStudents == 0;
            }
        }

        public async Task<bool> CreateTrainingRegistration(TrainingRegistrationDto trainingRegistrationDto)
        {
            using (var db = _dbContextFactory.CreateDbContext())
            {
                Training? training = await db.Trainings.FirstOrDefaultAsync(training => training.Id == trainingRegistrationDto.TrainingId);

                if (training.Active == false) return false;

                TrainingUser trainingUserRegistered = await db.TrainingUsers.FirstOrDefaultAsync(trainingUser => trainingUser.UserId == trainingRegistrationDto.UserId && trainingUser.TrainingId == trainingRegistrationDto.TrainingId);

                if (trainingUserRegistered != null) throw new Exception("Usuário já possui matrícula no treinamento");

                TrainingUser trainingUser = new TrainingUser();
                trainingUser.Completed = false;
                trainingUser.TrainingId = trainingRegistrationDto.TrainingId;
                trainingUser.UserId = trainingRegistrationDto.UserId;
                trainingUser.RegistrationDate = DateTime.UtcNow;
                db.TrainingUsers.Add(trainingUser);

                foreach (var topic in trainingRegistrationDto.TopicIds)
                {
                    TopicUser topicUser = new TopicUser();
                    topicUser.UserId = trainingRegistrationDto.UserId;
                    topicUser.TopicId = topic;
                    topicUser.Completed = false;
                    db.TopicUsers.Add(topicUser);
                }

                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}

