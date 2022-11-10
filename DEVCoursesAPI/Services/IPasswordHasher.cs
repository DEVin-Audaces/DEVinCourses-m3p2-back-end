namespace DEVCoursesAPI.Services;

public interface IPasswordHasher
{
    string CreateHash(string password);
    bool CheckHash(string hash, string password); 
}