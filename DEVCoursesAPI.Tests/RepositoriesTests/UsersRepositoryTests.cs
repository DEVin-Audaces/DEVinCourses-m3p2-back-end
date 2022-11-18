using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Tests.RepositoriesTests;

public class UsersRepositoryTests
{
       private Users GerarUsers()
    {
        Users usersModel = new Users() { Name = "Rodrigo Raiche", Email = "raiche@gmail.com", Age = 40, CPF = 256936580001, 
            Password = "R1515154", Image = null };
        return usersModel;
    }
    
    [Fact]
    public void Salvar_Users_No_Repository_Sucesso()
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Users usersModel = GerarUsers();
        
        //Act
        Guid idInseridoDoUsers = usersRepository.Add(usersModel);

        //Assert
        var query = usersRepository.GetEmail(usersModel.Email);
        Guid id =  query.Id;
        Assert.Equal(id, idInseridoDoUsers);

    }
    
    
    [Fact]
    public void  Salvar_Users_No_Repository_Com_Exception()
    {
        Action acaoSalvarComErro = () =>
        {
            //Arrange
            UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
            Users usersModel = new Users();

            //Act
            usersRepository.Add(usersModel);
        };

        //Assert
        Assert.Throws<DbUpdateException>(acaoSalvarComErro);
    }

    [Fact]
    public void Atualizar_Users_No_Repository_Sucesso()
    {
        //Arrange
        bool AtualizaTrue = true;
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Users usersModel = GerarUsers();
        Guid idInseridoDoUsers = usersRepository.Add(usersModel);
        usersModel.Id = idInseridoDoUsers; 

        //Act
        bool atualizado = usersRepository.Update(usersModel);
        
        //Assert
        Assert.Equal(AtualizaTrue, atualizado);

    }

    [Theory]
    [InlineData("8EE03886-4B79-4D03-2F8F-08DAC3618BAE")]
    public void Atualizar_Users_No_Repository_Sem_encontrar_User(Guid id)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Users usersModel = GerarUsers();
        usersModel.Id = id;

        //Act
        Action AtualizaAction = () =>
        {
            bool atualizado = usersRepository.Update(usersModel);
        };
        
        //Assert
        Assert.Throws<DbUpdateConcurrencyException>(AtualizaAction);

    }

    [Fact]
    public void  Buscar_Users_No_Repository_Por_Email_Encontrando()
    {
        //Arrange
        string email = "raiche@gmail.com";
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());
        Users usersModel = GerarUsers();
        usersRepository.Add(usersModel);

        //Act
        Users users = usersRepository.GetEmail(email);
        
        //Assert
        Assert.Equal(email, users.Email);
        
    }

    [Theory]
    [InlineData("raicheJose@gmail.com")]
    public void  Buscar_Users_No_Repository_Por_Email_Nao_Encontrando(string email)
    {
        //Arrange
        UsersRepository usersRepository = new UsersRepository(new TestCoursesDbContextFactory());

        //Act
        Func<object>  BuscaAction = () =>
        {
            Users users= usersRepository.GetEmail(email);
            return users.Email;
        };

        //Assert
        Assert.Throws<NullReferenceException>(BuscaAction);
       
        
    }

 
}