using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Data.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        [ForeignKey("Training")]
        public Guid TrainingId { get; set; }
        public Training? Training { get; set; }

    }
}
