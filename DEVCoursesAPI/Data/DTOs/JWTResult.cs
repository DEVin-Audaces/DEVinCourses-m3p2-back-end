namespace DEVCoursesAPI.Data.DTOs;

public class JWTResult
{
    public string AccessToken { get; set; }
    public DateTime ExpiresIn { get; set; }
}