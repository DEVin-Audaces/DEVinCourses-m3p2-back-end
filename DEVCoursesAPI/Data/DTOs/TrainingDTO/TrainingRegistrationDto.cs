namespace DEVCoursesAPI.Data.DTOs.TrainingDTO
{
    public class TrainingRegistrationDto
    {
        public Guid UserId { get; set; }
        public Guid TrainingId { get; set; }
        public IEnumerable<Guid> TopicIds { get; set; }
    }
}
