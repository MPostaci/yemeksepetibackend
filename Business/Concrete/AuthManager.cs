using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class AuthManager:IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private readonly IRedisCacheService _redisCacheService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IRedisCacheService redisCacheService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _redisCacheService = redisCacheService;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password,out passwordHash,out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                Name = userForRegisterDto.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            _userService.Add(user);
            return  new SuccessDataResult<User>(user,Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck==null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password,userToCheck.PasswordHash,userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck,Messages.SuccessfulLogin);
        }

        public async Task LogoutAsync(HttpRequest request, HttpResponse response, string userId)
        {
            var refreshToken = request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {                
                await _redisCacheService.Clear(refreshToken);
                
            }

            response.Cookies.Delete("accessToken");
            response.Cookies.Delete("refreshToken");
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email)!=null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<(AccessToken accessToken, RefreshToken refreshToken)> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var Tokens = _tokenHelper.CreateToken(user, claims);
            var refreshToken = Tokens.refreshToken;
            var accessToken = Tokens.accessToken;
            return new SuccessDataResult<(AccessToken accessToken, RefreshToken refreshToken)>((accessToken, refreshToken),Messages.AccessTokenCreated);
        }



        public void SetTokensInsideCookie(AccessToken accessToken, RefreshToken refreshToken, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", accessToken.Token,
                new CookieOptions
                {
                    Expires = accessToken.Expiration,
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                }
            );
            context.Response.Cookies.Append("refreshToken", refreshToken.Token,
                new CookieOptions
                {
                    Expires = refreshToken.Expiration,
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                }
            );
        }

    }
}
