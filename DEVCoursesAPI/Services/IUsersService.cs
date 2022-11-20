using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using Microsoft.Extensions.Options;

namespace DEVCoursesAPI.Services;

public interface IUsersService
{
    Guid Add(DataUser user);
    JWTResult AuthUser(LoginUser login);

    public Guid GetIdToken(string authHeader);

    bool Update(DataUser user, Guid id);
    
    bool UploadImg(UploadImgUser img, Guid id);

    bool ResetPassword(LoginUser login);


}