
using DEVCoursesAPI.Data.Models;

namespace DEVCoursesAPI.Repositories {
    public interface ITopicsRepository {
        Task<Guid> CreateAsync(Topic topic);
    }
}