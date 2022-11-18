using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories
{
    public class ModulesRepository : IModulesRepository
    {
        private readonly IDbContextFactory<DEVCoursesContext> _dbFactory;

        public ModulesRepository(IDbContextFactory<DEVCoursesContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Guid> CreateModuleAsync(Module module)
        {
            using (var db = _dbFactory.CreateDbContext())
            {
                db.Modules.Add(module);
                await db.SaveChangesAsync();

                return module.Id;
            }
        }
    }
}