using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVCoursesAPI.Tests.RepositoriesTests
{
    public class TrainingRepositoryTests
    {
        private readonly DbContextOptions<DEVCoursesContext> _contextOptions;

        public TrainingRepositoryTests()
        {
            _contextOptions = new DbContextOptionsBuilder<DEVCoursesContext>()
                .UseInMemoryDatabase("TrainingRepositoryTests")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new DEVCoursesContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public void GetByIdAsync_Success()
        {
            var context = new DEVCoursesContext(_contextOptions);
            var qualquerNome = context.Trainings.ToList();

            var training = context.Trainings.Where(training => training.Id == Guid.NewGuid()).Count();

            Assert.Equal(1, training);
        }

    }
}
