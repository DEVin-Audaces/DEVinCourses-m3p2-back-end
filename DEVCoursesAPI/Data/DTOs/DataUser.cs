using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Data.DTOs;

public class DataUser
{
    [Required(ErrorMessage = "O Campo nome é obrigatório.")]
    [StringLength(255, ErrorMessage = "O tamanho máximo do campo nome é de 255 caracteres")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "O campo Email é obrigatório")]
    [StringLength(150, ErrorMessage = "O tamanho máximo do campo nome é de 150 caracteres")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "O campo Idade é obrigatório")]
    public  int Age { get; set; }
    
    [Required(ErrorMessage = "O campo CPF é obrigatório")]
    public  long CPF { get; set; }
    
    [Required(ErrorMessage = "O campo Senha é obrigatório")]
    public string Password { get; set; }

    [Required(ErrorMessage = "O campo Senha de conferência é obrigatório")]
    public string PasswordRepeat { get; set; }
}   
