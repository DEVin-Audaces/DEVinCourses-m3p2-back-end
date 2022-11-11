namespace DEVCoursesAPI.Repositories;

public interface IEntity<TModel>
{
    Guid Add(TModel model);
    bool Update(TModel model);
}

