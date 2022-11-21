using DEVCoursesAPI.Data.DTOs.ModuleDTO;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services
{
    public class ModulesService : IModulesService
    {
        private readonly IModulesRepository _modulesRepository;
        private readonly ITopicsService _topicsService;

        public ModulesService(IModulesRepository modulesRepository, ITopicsService topicsService)
        {
            _modulesRepository = modulesRepository;
            _topicsService = topicsService;
        }
        public async Task CreateModulesAsync(CreateModuleDto moduleDto, Guid trainingId)
        {
            Module module = (Module) moduleDto;
            module.TrainingId = trainingId;
            Guid moduleId = await _modulesRepository.CreateModuleAsync(module);

            foreach (var topic in moduleDto.Topics)
            {
                await _topicsService.CreateTopicsAsync(topic, moduleId);
            }
        }
    }
}