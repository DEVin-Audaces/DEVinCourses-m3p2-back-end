namespace DEVCoursesAPI.Data.DTOs;

public class ProfileUser
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public  int Age { get; set; }
    
    public  long CPF { get; set; }
    
    public string? Image { get; set; }

}