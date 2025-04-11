using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;

namespace Business.Abstract
{
    public interface IAuthService
    {
        //IDataResult<User> GetUser(int userId);
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto,string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IDataResult<(AccessToken accessToken, RefreshToken refreshToken)> CreateAccessToken(User user);
        void SetTokensInsideCookie(AccessToken accessToken, RefreshToken refreshToken, HttpContext context);
        Task LogoutAsync(HttpRequest request, HttpResponse response, string userId);

    }
}
