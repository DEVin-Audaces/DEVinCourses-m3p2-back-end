using System.IdentityModel.Tokens.Jwt;
using System.Text;
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


    public Guid Add(CreateUser user)
    {
        throw new NotImplementedException();
    }

    public bool Update(Users user)
    {
        throw new NotImplementedException();
    }

    public JWTResult AuthUser(LoginUser login )
    {
        var currentUser = _usersRepository.GetEmail(login.Email);
        
        if (currentUser == null)
            throw new Exception("Usuário não encontrado");
        
        bool validatePassword = _passwordHasher.CheckHash(
            currentUser.Password, login.Password
        );

        if (!validatePassword)
            throw new Exception("Senha inválida");
        
        var generatedIn = DateTime.UtcNow;
        var expiresIn = generatedIn.AddHours(1);
        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Value.Key));
        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
        var handler = new JwtSecurityTokenHandler();

        var tokenProperties = new SecurityTokenDescriptor()
        {
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
}