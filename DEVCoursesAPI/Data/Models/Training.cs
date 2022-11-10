using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
    }
}
