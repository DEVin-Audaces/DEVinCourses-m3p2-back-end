using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories;

public interface IUsersRepository<TEntity> : IEntity<TEntity>
{
    Users GetEmail(string email);
}