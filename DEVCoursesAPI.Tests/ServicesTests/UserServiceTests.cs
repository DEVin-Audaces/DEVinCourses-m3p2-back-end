using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace DEVCoursesAPI.Tests.ServicesTests;

[TestCaseOrderer("DEVCoursesAPI.Tests.AlphabeticalOrder", "DEVCoursesAPI.Tests")]

public class UserServiceTests
{
    [Theory]
    [InlineData("Rodrigo Raiche", 18, "AAAA4444", "AAAA4444")]
    public void A_Adicionar_Users_Com_Informacoes_Validas(string name, int age,  string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$/8g9noaqQnsO4cy1aDPpcw$RBho86S4TAqcHnvvY0ljbP6mkTDlSG/Ub9Bcqbnrt1U");
        
        Random randNum = new Random();
        long CPF = randNum.Next();

        string email = "rodrigoraiche" + CPF + "@gmail.com";
        
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        Guid id = usersService.Add(user);
            
        
        //Assert
        Assert.NotEqual(new Guid(), id);
    }
    
    [Theory]
    [InlineData("", "", 0, 0, "", "")]
    public void B_Adicionar_Users_Com_CPF_Invalido_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "CPF é obrigatório";
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Add(user); });
            
        //Assert
        Assert.Equal(message, exception.Message);
    }



    [Theory]
    [InlineData("", "", 0, 2569024699, "", "")]
    [InlineData("ab", "", 0, 2569024699, "", "")]
    public void C_Adicionar_Users_Com_Nome_Invalido_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Nome deve possuir no mínimo 3 caracteres e apenas caracteres alfabético";
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Add(user); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }
    
    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "A", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "A4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AA4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAA4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA44", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAAAAA", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "4444444", "AAAA4444")]
    public void D_Adicionar_Users_Com_Senha_Invalida_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Senha deve ter pelo menos 8 dígitos alfanuméricos (letras e números)";
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Add(user); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }


    [Theory]
    [InlineData("Rodrigo Raiche", "", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmailcom", 0, 2569024699, "AAAA4444", "AAAA4444")]
    public void E_Adicionar_Users_Com_Email_Invalido_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "E-mail possui formato inválido";
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Add(user); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 17, 2569024699, "AAAA4444", "AAAA4444")]
    public void F_Adicionar_Users_Com_Idade_Invalida_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Usuário deverá possuir idade maior ou igual a 18 anos";
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Add(user); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche1@gmail.com", 18, 2569024699, "AAAA4444", "AAAA4445")]
    public void G_Adicionar_Users_Com_Senha_Nao_Confere_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "As senhas não conferem";
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Add(user); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }
   

    [Theory]
    [InlineData("rodrigoraiche2@gmail.com", "RRR1525B")]
    public void H_Autenticacao_Com_Sucesso(string email, string password)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$HAY947bGo5giBJbPLQwMjg$sxJkVpzCKP8pRhaB3slQs8cYeyxcJAlZE94bzHrGb0Y");
        var tokens = new TokenSettings
        {
            Issuer = "localhost:7200",
            Audience = "DEVCoursesAPI",
            Key = "QSW3U1ezrO@62OXW5I^XpXVC4GkzpGE#P&%g5z*ZuciN^qw6JD"
        };
        tokenSettings.Setup(s => s.Value).Returns(tokens);
        
        var user = new DataUser
        {
            Name = "Bia", Email = "rodrigoraiche2@gmail.com", Age = 19, CPF = 02569623155, Password = "RRR1525B", PasswordRepeat = "RRR1525B"
        };
        usersService.Add(user);
        var login = new LoginUser {
            Email = email, Password = password
        };
        passwordHasher.Setup(s => s.CheckHash(It.IsAny<string>(),It.IsAny<string>())).Returns(true);

        //Act
        var token  = usersService.AuthUser(login);
            
        //Assert
        Assert.NotEqual("", token.AccessToken);

    }

    [Theory]
    [InlineData("rodrigoraiche3@gmail.com", "RRR1525B")]
    public void I_Autenticacao_Com_Senha_Invalida_Exception(string email, string password)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$HAY947bGo5giBJbPLQwMjg$sxJkVpzCKP8pRhaB3slQs8cYeyxcJAlZE94bzHrGb0Y");
        var tokens = new TokenSettings
        {
            Issuer = "localhost:7200",
            Audience = "DEVCoursesAPI",
            Key = "QSW3U1ezrO@62OXW5I^XpXVC4GkzpGE#P&%g5z*ZuciN^qw6JD"
        };
        tokenSettings.Setup(s => s.Value).Returns(tokens);
        
        var user = new DataUser
        {
            Name = "Bia", Email = "rodrigoraiche3@gmail.com", Age = 19, CPF = 02569644155, Password = "RRR1525B", PasswordRepeat = "RRR1525B"
        };
        usersService.Add(user);
        var login = new LoginUser {
            Email = email, Password = password
        };
        string message = "Usuário ou senha inválidos";
        passwordHasher.Setup(s => s.CheckHash(It.IsAny<string>(),It.IsAny<string>())).Returns(false);

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.AuthUser(login); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("rodrigoraiche4@gmail.com", "AAAA4444")]
    public void J_Autenticacao_Nao_Encontrado_User_Exception(string email, string password)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Usuário não encontrado";
        var login = new LoginUser {
            Email = email, Password = password
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.AuthUser(login); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }


    [Theory]
    [InlineData("rodrigoraiche4@gmail.com", "AAAA4444")]
    public void K_Reset_Password_Com_Sucesso(string email, string password)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$/8g9noaqQnsO4cy1aDPpcw$RBho86S4TAqcHnvvY0ljbP6mkTDlSG/Ub9Bcqbnrt1U");
        var user = new DataUser
        {
            Name = "Bia", Email = "rodrigoraiche4@gmail.com", Age = 19, CPF = 01269644155, Password = "AAAA5555", PasswordRepeat = "AAAA5555"
        };
        usersService.Add(user);
        var login = new LoginUser {
            Email = email, Password = password
        };

        //Act
        var resultado  = usersService.ResetPassword(login);
            
        //Assert
        Assert.Equal(true, resultado);

    }

    [Theory]
    [InlineData("rodrigoraiche5@gmail.com", "AAAA4444")]
    public void L_Reset_Password_Nao_Encontrado_User_Exception(string email, string password)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Usuário não encontrado";
        var login = new LoginUser {
            Email = email, Password = password
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.ResetPassword(login); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    
    [Theory]
    [InlineData("Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InJhaWNoZUBnbWFpbC5jb20iLCJqdGkiOiI1NjkxYjM5YS1iOTY4LTQxODctODAwOS0wOGRhY2E0ZTdiYzgiLCJuYmYiOjE2Njg4OTg4NTksImV4cCI6MTY2ODkyNzY1OSwiaWF0IjoxNjY4ODk4ODU5LCJpc3MiOiJsb2NhbGhvc3Q6NzIwMCIsImF1ZCI6IkRFVkNvdXJzZXNBUEkifQ.0uyCkb9mBGX3yquiwTo-QMO8t5xJqJakWnAkIAjJRXc")]
    public void M_Obter_Id_Token_Sucesso(string token)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        //Act
        var idresult = usersService.GetIdToken(token);
            
        //Assert
        Assert.Equal(id, idresult);

    }

    
    [Theory]
    [InlineData("", "", 0, 0, "", "")]
    public void N_Atualiza_Users_Com_CPF_Invalido_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "CPF é obrigatório";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");

        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);
    }



    [Theory]
    [InlineData("", "", 0, 2569024699, "", "")]
    [InlineData("ab", "", 0, 2569024699, "", "")]
    public void O_Atualiza_Users_Com_Nome_Invalido_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Nome deve possuir no mínimo 3 caracteres e apenas caracteres alfabético";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }
    
    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "A", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "A4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AA4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAA4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA4", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA44", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAAAAA", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "4444444", "AAAA4444")]
    public void P_Atualiza_Users_Com_Senha_Invalida_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Senha deve ter pelo menos 8 dígitos alfanuméricos (letras e números)";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }


    [Theory]
    [InlineData("Rodrigo Raiche", "", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmailcom", 0, 2569024699, "AAAA4444", "AAAA4444")]
    public void Q_Atualiza_Users_Com_Email_Invalido_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "E-mail possui formato inválido";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 0, 2569024699, "AAAA4444", "AAAA4444")]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 17, 2569024699, "AAAA4444", "AAAA4444")]
    public void R_Atualiza_Users_Com_Idade_Invalida_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Usuário deverá possuir idade maior ou igual a 18 anos";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche6@gmail.com", 18, 2569024699, "AAAA4444", "AAAA4445")]
    public void S_Atualiza_Users_Com_Senha_Nao_Confere_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "As senhas não conferem";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche@gmail.com", 18, 2569024699, "AAAA4444", "AAAA4444")]
    public void T_Atualiza_Users_Nao_Encontrado_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Usuário não encontrado";
        var id = new Guid("5691b39a-b968-4187-8009-08daca4e7bc8");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(user, id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }
    
    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraich3@gmail.com", 18, 2569024670, "AAAA4444", "AAAA4444")]
    public void U_Atualiza_Users_Email_Alterado_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$/8g9noaqQnsO4cy1aDPpcw$RBho86S4TAqcHnvvY0ljbP6mkTDlSG/Ub9Bcqbnrt1U");
        var message = "Não é permitido a troca do E-mail";

        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };
        Guid idNew = usersService.Add(user);
        var userAlterado = new DataUser
        {
            Name = "Rodrigo Raiche", Email = "rodrigoraiche2@gmail.com", Age = 19, CPF = CPF, Password = "AAAA4445", PasswordRepeat = "AAAA4445"
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(userAlterado, idNew); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigo@gmail.com", 18, 2569024699, "AAAA4444", "AAAA4444")]
    public void V_Atualiza_Users_CPF_Alterado_Exception(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$/8g9noaqQnsO4cy1aDPpcw$RBho86S4TAqcHnvvY0ljbP6mkTDlSG/Ub9Bcqbnrt1U");
        var message = "Não é permitido a troca do CPF";

        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };
        Guid idNew = usersService.Add(user);
        var userAlterado = new DataUser
        {
            Name = "Bia", Email = email, Age = 19, CPF = 2569024698, Password = "AAAA4445", PasswordRepeat = "AAAA4445"
        };

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Update(userAlterado, idNew); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }

    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche7@gmail.com", 18, 2569012399, "AAAA4444", "AAAA4444")]
    public void W_Atualiza_Users_Sucesso(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$/8g9noaqQnsO4cy1aDPpcw$RBho86S4TAqcHnvvY0ljbP6mkTDlSG/Ub9Bcqbnrt1U");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };
        Guid idNew = usersService.Add(user);
        var userAlterado = new DataUser
        {
            Name = "Bia", Email = email, Age = 19, CPF = CPF, Password = "AAAA4445", PasswordRepeat = "AAAA4445"
        };

        //Act
        var resultado = usersService.Update(userAlterado, idNew); 
            
        //Assert
        Assert.Equal(true, resultado);

    }

    
    [Theory]
    [InlineData("Rodrigo Raiche", "rodrigoraiche8@gmail.com", 18, 2123024699, "AAAA4444", "AAAA4444")]
    public void X_Obter_Users_Encontrado_Sucesso(string name, string email, int age, long CPF, string password, string passwordRepeat)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        passwordHasher.Setup(s => s.CreateHash(It.IsAny<string>())).Returns("$argon2id$v=19$m=65536,t=3,p=1$/8g9noaqQnsO4cy1aDPpcw$RBho86S4TAqcHnvvY0ljbP6mkTDlSG/Ub9Bcqbnrt1U");
        var user = new DataUser
        {
            Name = name, Email = email, Age = age, CPF = CPF, Password = password, PasswordRepeat = passwordRepeat
        };
        Guid idNew = usersService.Add(user);

        //Act
        var profile = usersService.Get(idNew);
            
        //Assert
        Assert.Equal(email, profile.Email);

    }

    [Fact]
    public void Y_Obter_Users_Nao_Encontrado_Exception()
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Mock<IOptions<TokenSettings>> tokenSettings = new Mock<IOptions<TokenSettings>>();
        Mock<IPasswordHasher> passwordHasher = new Mock<IPasswordHasher>();
        var usersService = new UsersService(usersRepository, tokenSettings.Object, passwordHasher.Object);
        var message = "Usuário não encontrado";
        var id = new Guid("9f5c683d-bE1b-4078-2fD4-08daca7a1db8");

        //Act
        var exception = Assert.Throws<Exception>(() => { usersService.Get(id); });
            
        //Assert
        Assert.Equal(message, exception.Message);

    }
    
}