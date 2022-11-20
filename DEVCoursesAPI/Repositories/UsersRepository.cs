using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DEVCoursesAPI.Repositories;

public class UsersRepository : IUsersRepository<Users>
{
    private readonly IDbContextFactory<DEVCoursesContext> _dbContextFactory;

    public UsersRepository(IDbContextFactory<DEVCoursesContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }


    public Guid Add(Users user)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.Add(user);
            context.SaveChanges();
            return user.Id;
        }
    }

    public bool Update(Users model)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            context.Update(model);
            return context.SaveChanges() > 0;
        }
    }

    public Users GetEmail(string email)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return  context.Users.Where(q => q.Email.ToLower() == email.ToLower()).FirstOrDefault(); 
        }
    }

    public Users GetId(Guid id)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return  context.Users.Where(q => q.Id == id).FirstOrDefault();
        }    }

    public Users GetCPF(Double cpf)
    {
        using (var context = _dbContextFactory.CreateDbContext())
        {
            return  context.Users.Where(q => q.CPF == cpf).FirstOrDefault();
        }
    }
}
    
