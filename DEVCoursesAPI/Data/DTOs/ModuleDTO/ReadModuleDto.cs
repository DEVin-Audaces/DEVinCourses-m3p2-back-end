using DEVCoursesAPI.Data.DTOs.TopicDTO;

namespace DEVCoursesAPI.Data.DTOs.ModuleDTO
{
    public class ReadModuleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public IEnumerable<ReadTopicDto> Topics { get; set; }
    }
}
