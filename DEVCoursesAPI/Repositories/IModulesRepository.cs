using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories
{
    public interface IModulesRepository
    {
        Task<Guid> CreateModuleAsync(Module module);
    }
}