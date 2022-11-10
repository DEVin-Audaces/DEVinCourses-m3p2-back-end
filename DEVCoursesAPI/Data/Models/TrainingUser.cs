using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEVCoursesAPI.Data.Models
{
    public class TrainingUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Completed { get; set; }
        public DateTime RegistrationDate { get; set; }

        [ForeignKey("Training")]
        public Guid TrainingId { get; set; }
        public Training? Training { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }
        public Users? Users { get; set; }
    }
}