namespace DEVCoursesAPI.Data.DTOs.TopicDTO {
    public class ReadTopicDto {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
    }
}