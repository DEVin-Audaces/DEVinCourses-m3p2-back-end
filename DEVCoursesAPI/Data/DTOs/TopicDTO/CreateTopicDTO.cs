using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Data.DTOs.TopicDTO {
    public class CreateTopicDto {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }

        public static explicit operator Topic(CreateTopicDto dto) {
            return new Topic {
                Type = dto.Type,
                Name = dto.Name,
                Content = dto.Content,
                Index = dto.Index
            };
        }
    }
}