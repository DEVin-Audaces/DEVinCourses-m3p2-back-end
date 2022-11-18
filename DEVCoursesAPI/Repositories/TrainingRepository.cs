using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

            FilteredList.ForEach(x => Console.WriteLine(x.Instructor + "    " + x.Id));
            return FilteredList;

            





            //var trainings = _context.Trainings.ToList();
            //var trainingsUsers = _context.TrainingUsers.Where(x => x.UserId == userId).ToList();
            //var listaTreinamentosNaoMatriculado = new List<Training>();

            //List<TrainingNotRegistered> FilteredList = new List<TrainingNotRegistered>();

            //foreach (var training in trainings)
            //{
            //    foreach (var trainingUser in trainingsUsers)
            //    {
            //        if (training.Id == trainingUser.TrainingId)
            //        {
            //            var newTraining = new TrainingNotRegistered()
            //            {
            //                Id = training.Id,
            //                Name = training.Name,
            //                Summary = training.Summary,
            //                Duration = training.Duration,
            //                Instructor = training.Instructor,
            //                Author = training.Author,
            //                Active = training.Active
            //            };
            //            FilteredList.Add(newTraining);

            //        }else
            //        {

            //        }

            //    }
            //}

            //trainings.ForEach(training => 
            //{
            //    var newTraining = new TrainingNotRegistered()
            //    {
            //        Id = training.Id,
            //        Name = training.Name,
            //        Summary = training.Summary,
            //        Duration = training.Duration

            //    };
            //    FilteredList.Add(newTraining);
            //});

            //return FilteredList;
        }
    }
}
