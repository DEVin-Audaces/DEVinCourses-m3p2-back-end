using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVCoursesAPI.Tests.RepositoriesTests
{
    public class TestModulesRepository
    {

        [Fact]
        public async void CreateModuleAsync_ShouldReturnGuidWhenCreatingModule()
        {

            IModulesRepository repo = new ModulesRepository(new TestCoursesDbContextFactory());
            Module module = new() { Name = "Name", Index = 0 };

            Guid result = await repo.CreateModuleAsync(module);

            Assert.IsType<Guid>(result);
        }

    }
}
