using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DEVCoursesAPI.Data.DTOs.TopicDTO;

namespace DEVCoursesAPI.Data.Models
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
        [ForeignKey("Module")]
        public Guid ModuleId { get; set; }
        public Module? Module { get; set; }

        public static explicit operator ReadTopicDto(Topic topic)
        {
            return new ReadTopicDto
            {
                Id = topic.Id,
                Type = topic.Type,
                Name = topic.Name,
                Content = topic.Content,
                Index = topic.Index
            };
        }
    }
}
