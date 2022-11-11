﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEVCoursesAPI.Data.Models
{
    public class TopicUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Completed { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }
        public Users? Users { get; set; }

        [ForeignKey("Topic")]
        public Guid TopicId { get; set; }
        public Topic? Topic { get; set; }
  
        public Guid TrainingId { get; set; }
        public Training? Training { get; set; }

    }
}