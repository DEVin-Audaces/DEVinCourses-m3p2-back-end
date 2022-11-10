namespace DEVCoursesAPI.Data.Models;

public class Users
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public  int Age { get; set; }
    
    public  long CPF { get; set; }
    
    public string Password { get; set; }
    
    public byte[]? Image { get; set; }

    
}