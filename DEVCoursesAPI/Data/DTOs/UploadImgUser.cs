using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Data.DTOs;

public class UploadImgUser
{
    [Required]
    [DataType(DataType.Upload)]
    public IFormFile ImageUpload { get; set; }
}