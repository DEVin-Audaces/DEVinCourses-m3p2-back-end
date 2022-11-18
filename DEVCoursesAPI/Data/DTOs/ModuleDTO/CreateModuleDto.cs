using DEVCoursesAPI.Data.DTOs.TopicDTO;
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Data.DTOs.ModuleDTO
{
    public class CreateModuleDto
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public ICollection<CreateTopicDto> Topics { get; set; }

        public static explicit operator Module (CreateModuleDto dto)
        {
            return new Module
            {
                Name = dto.Name,
                Index = dto.Index
            };
        }
    }
}
