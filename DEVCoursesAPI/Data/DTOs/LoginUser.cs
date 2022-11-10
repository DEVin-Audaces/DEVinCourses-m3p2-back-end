using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Data.DTOs;

public class LoginUser
{
    [Required(ErrorMessage = "O campo Email é obrigatório")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "O campo Senha é obrigatório")]
    public string Password { get; set; }
}