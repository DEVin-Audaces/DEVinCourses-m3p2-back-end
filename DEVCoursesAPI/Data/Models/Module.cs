using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DEVCoursesAPI.Data.DTOs.ModuleDTO;
using DEVCoursesAPI.Data.DTOs.TopicDTO;

namespace DEVCoursesAPI.Data.Models
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public List<Topic> Topics { get; set; }
        [ForeignKey("Training")]
        public Guid TrainingId { get; set; }
        public Training? Training { get; set; }

        public static explicit operator ReadModuleDto(Module module)
        {
            return new ReadModuleDto
            {
                Id = module.Id,
                Name = module.Name,
                Index = module.Index,
                Topics = module.Topics.OrderBy(t => t.Index).Select(t => (ReadTopicDto)t),
            };
        }
    }
}
