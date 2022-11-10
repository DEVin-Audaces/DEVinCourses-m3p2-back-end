using System.ComponentModel.DataAnnotations.Schema;

namespace DEVCoursesAPI.Data.Models
{
    public class TrainingUser
    {
        public Guid Id { get; set; }
        [ForeignKey("Training")]
        public Guid TrainingId { get; set; }
        [ForeignKey("Users")]
        public Guid UserId { get; set; }
        public bool Completed { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Training? Training { get; set; }
        public Users? Users { get; set; }
    }
}