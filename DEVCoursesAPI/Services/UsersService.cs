using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Services;

public class UsersService: IUsersService 
{
    private readonly IUsersRepository<Users> _usersRepository;

    public UsersService(IUsersRepository<Users> usersRepository)
    {
        _usersRepository = usersRepository;
    }


    public Guid Add(CreateUser user)
    {
        throw new NotImplementedException();
    }

    public bool Update(Users user)
    {
        throw new NotImplementedException();
    }

    public IList<Users> GetAll()
    {
        throw new NotImplementedException();
    }
}