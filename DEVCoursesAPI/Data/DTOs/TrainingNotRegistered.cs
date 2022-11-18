using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEVCoursesAPI.Data.DTOs
{
    public class TrainingNotRegistered
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public double Duration { get; set; }
        public string? Instructor { get; set; }
        public Guid? Author { get; set; }
        public bool? Active { get; set; }

    }
}
