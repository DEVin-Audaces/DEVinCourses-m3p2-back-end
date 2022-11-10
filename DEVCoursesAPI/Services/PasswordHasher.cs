using Isopoh.Cryptography.Argon2;

namespace DEVCoursesAPI.Services;

public class PasswordHasher : IPasswordHasher
{
    public string CreateHash(string password)
    {
        return Argon2.Hash(password);
    }

    public bool CheckHash(string hash, string password)
    {
        return Argon2.Verify(hash, password);
    }
}