using DEVCoursesAPI.Data.DTOs.ModuleDTO;
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Data.DTOs.TrainingDTO
{
    public class ReadTrainingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public double Duration { get; set; }
        public string Instructor { get; set; }
        public Guid Author { get; set; }
        public bool Active { get; set; }
        public IEnumerable<ReadModuleDto> Modules { get; set; }
    }
}