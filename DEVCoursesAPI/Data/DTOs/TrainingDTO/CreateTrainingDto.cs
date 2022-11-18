using DEVCoursesAPI.Data.DTOs.ModuleDTO;
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Data.DTOs.TrainingDTO
{
    public class CreateTrainingDto
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public double Duration { get; set; }
        public string Instructor { get; set; }
        public Guid Author { get; set; }
        public bool Active { get; set; }
        public ICollection<CreateModuleDto> Modules { get; set; }
        public static explicit operator Training(CreateTrainingDto dto)
        {
            return new Training
            {
                Name = dto.Name,
                Summary = dto.Summary,
                Duration = dto.Duration,
                Instructor = dto.Instructor,
                Author = dto.Author,
                Active = dto.Active
            };
        }
    }
}