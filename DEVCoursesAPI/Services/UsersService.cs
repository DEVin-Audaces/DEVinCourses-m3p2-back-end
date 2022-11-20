using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DEVCoursesAPI.Services;

public class UsersService: IUsersService 
{
    private readonly IUsersRepository<Users> _usersRepository;
    private readonly IOptions<TokenSettings> _tokenSettings;
    private readonly IPasswordHasher _passwordHasher;


    public UsersService(IUsersRepository<Users> usersRepository, IOptions<TokenSettings> tokenSettings, IPasswordHasher passwordHasher)
    {
        _usersRepository = usersRepository;
        _tokenSettings = tokenSettings;
        _passwordHasher = passwordHasher;
    }


    public Guid Add(DataUser user)
    {
        validateUser(user);
        
        Users currentUser = _usersRepository.GetEmail(user.Email);
        
        if (currentUser != null )
            throw new Exception("E-mail já está cadastrado na base de dados");
        
        currentUser = _usersRepository.GetCPF(user.CPF);
        
        if (currentUser != null )
            throw new Exception("CPF já está cadastrado na base de dados");

       
        var newUser = new Users { Age = user.Age, Email = user.Email, Name = user.Name, 
            Password = _passwordHasher.CreateHash(user.Password), CPF = user.CPF, Image = null};
        _usersRepository.Add(newUser);

        return newUser.Id;
    }

    public bool Update(Users user)
    {
        throw new NotImplementedException();
    }

    public JWTResult AuthUser(LoginUser login )
    {
        var currentUser = this.UserSearchEmail(login.Email);
        
        bool validatePassword = _passwordHasher.CheckHash(
            currentUser.Password, login.Password
        );

        if (!validatePassword)
            throw new Exception("Usuário ou senha inválidos");

        return CreateToken(currentUser);
    }

    public Guid GetIdToken(string authHeader)
    {
        throw new NotImplementedException();
    }

    public bool Update(DataUser user, Guid id)
    {
        throw new NotImplementedException();
    }

    private Users UserSearchEmail(string email)
    {
        var currentUser = _usersRepository.GetEmail(email);
        
        if (currentUser == null)
            throw new Exception("Usuário não encontrado");

        return currentUser;
    }

    private JWTResult CreateToken(Users currentUser)
    {
        var generatedIn = DateTime.UtcNow;
        var expiresIn = generatedIn.AddHours(8);
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Value.Key));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
        var handler = new JwtSecurityTokenHandler();
        var identity = GetClaimsIdentity(currentUser);

        var tokenProperties = new SecurityTokenDescriptor()
        {
            Subject = identity,
            Issuer = _tokenSettings.Value.Issuer,
            IssuedAt = generatedIn,
            Expires = expiresIn,
            Audience = _tokenSettings.Value.Audience,
            SigningCredentials = credentials
        };

        var accessToken = handler.WriteToken(
            handler.CreateToken(tokenProperties)
        );

        return new JWTResult
        {
            AccessToken = accessToken,
            ExpiresIn = expiresIn
        };

    }
    
    private static ClaimsIdentity GetClaimsIdentity(Users currentUser)
    {
        return new ClaimsIdentity
        (
            new GenericIdentity(currentUser.Email),
            new[] {
                new Claim(JwtRegisteredClaimNames.Jti, currentUser.Id.ToString())
            }
        );
    }

    public bool ResetPassword(LoginUser login)
    {
        var currentUser = this.UserSearchEmail(login.Email);

        if (currentUser == null)
            throw new Exception("Usuário não encontrado");


        this.ValidatePassword(login.Password);

        currentUser.Password = _passwordHasher.CreateHash(login.Password);

        return _usersRepository.Update(currentUser);

    }

    private void ValidatePassword(string password)
    {
        Regex regex = new Regex(@"(^(?=.*\d)(?=.*[a-zA-Z])(?:([0-9a-zA-Z])(?!\1)){8,}$)", RegexOptions.IgnorePatternWhitespace);

        Match m = regex.Match(password);

        if (!m.Success)
            throw new Exception("Senha deve ter pelo menos 8 dígitos alfanuméricos (letras e números)");

    }

    private void validateUser(DataUser user)

    {
        if (user.CPF == 0)
            throw new Exception("CPF é obrigatório");
        
        if (user.Name.Length < 3)
            throw new Exception("Nome deve possuir no mínimo 3 caracteres e apenas caracteres alfabético");

        Regex regex = new Regex(@"^[a-záéíóúõãçàâêô ]+$", RegexOptions.IgnoreCase);

        Match m = regex.Match(user.Name);
        
        if (!m.Success)
            throw new Exception("Nome deve possuir no mínimo 3 caracteres e apenas caracteres alfabético");

        this.ValidatePassword(user.Password);

        
        regex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

        m = regex.Match(user.Email);
        
        if (!m.Success)
            throw new Exception("E-mail possui formato inválido");
        
        if (user.Age < 18 )
            throw new Exception("Usuário deverá possuir idade maior ou igual a 18 anos");

        if (user.Password != user.PasswordRepeat )
            throw new Exception("As senhas não conferem");

    }

}