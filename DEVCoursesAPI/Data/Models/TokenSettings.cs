using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Data.Models;

public class TokenSettings
{
    [Key]
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
}