using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;

namespace DEVCoursesAPI.Data.Models
{
    public class Training
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public double Duration { get; set; }
        public string Instructor { get; set; }
        public Guid Author { get; set; }
        public bool Active { get; set; }
        public List<Module> Modules { get; set; }

        public static explicit operator ReadTrainingDto(Training training)
        {
            return new ReadTrainingDto
            {
                Id = training.Id,
                Name = training.Name,
                Summary = training.Summary,
                Duration = training.Duration,
                Instructor = training.Instructor,
                Author = training.Author,
                Active = training.Active,
                Modules = training.Modules.OrderBy(m => m.Index).Select(m => (ReadModuleDto)m),
            };
        }
    }
}
