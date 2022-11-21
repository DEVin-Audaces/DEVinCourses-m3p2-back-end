using DEVCoursesAPI.Data.DTOs.ModuleDTO;

namespace DEVCoursesAPI.Services
{
    public interface IModulesService
    {
        Task CreateModulesAsync(CreateModuleDto moduleDto, Guid trainingId);
    }
}