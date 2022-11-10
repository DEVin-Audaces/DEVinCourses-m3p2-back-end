using System.ComponentModel.DataAnnotations.Schema;

namespace DEVCoursesAPI.Data.Models
{
    public class TopicUser
    {
        public Guid Id { get; set; }
        [ForeignKey("Training")]
        public Guid TrainingId { get; set; }
        [ForeignKey("Users")]
        public Guid UserId { get; set; }
        [ForeignKey("Topic")]
        public Guid TopicId { get; set; }
        public bool Completed { get; set; }
    }
}
