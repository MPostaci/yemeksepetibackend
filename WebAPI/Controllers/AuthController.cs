using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IRedisCacheService _redisCacheService;
        private IUserService _userService;

        public AuthController(IAuthService authService, IRedisCacheService redisCacheService, IUserService userService)
        {
            _authService = authService;
            _redisCacheService = redisCacheService;
            _userService = userService;
        }

        //[HttpGet("getuserid")]
        //public int GetUserId()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        throw new UnauthorizedAccessException("User not authenticated");
        //    }

        //    return int.Parse(userId);
        //}

        [HttpGet("checkauth")]
        [Authorize]
        public IActionResult CheckAuth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Id = userId,
                Role = role,
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = _authService.CreateAccessToken(userToLogin.Data);

            await StoreRefreshToken(userToLogin.Data.Id, result.Data.refreshToken);

            _authService.SetTokensInsideCookie(result.Data.accessToken, result.Data.refreshToken, HttpContext);

            if (result.Success)
            {
                return Ok(result.Data.accessToken);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            var result = _authService.CreateAccessToken(registerResult.Data);

            await StoreRefreshToken(registerResult.Data.Id, result.Data.refreshToken);

            _authService.SetTokensInsideCookie(result.Data.accessToken, result.Data.refreshToken, HttpContext);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _authService.LogoutAsync(Request, Response, userId);
            return Ok(new { message = "Logged out successfully" });
        }

        public string GetRefreshTokenFromCookie()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out string refreshToken))
            {
                return refreshToken;
            }

            return null;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshTokenFromCookie = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshTokenFromCookie))
            {
                return Unauthorized(new { Messsage = "Refresh token is missing" });
            }

            var userId = await _redisCacheService.GetValueAsync(refreshTokenFromCookie);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Invalid refresh token." });
            }

            var user = _userService.GetById(Convert.ToInt32(userId));

            if (user == null)
            {
                return Unauthorized(new { Message = "User not found" });
            }

            var Tokens = _authService.CreateAccessToken(user);

            await _redisCacheService.Clear(refreshTokenFromCookie);

            await StoreRefreshToken(user.Id, Tokens.Data.refreshToken);

            _authService.SetTokensInsideCookie(Tokens.Data.accessToken, Tokens.Data.refreshToken, HttpContext);

            return Ok(new { Message = "Token refreshed succesfully" });
        }

        private async Task StoreRefreshToken(int userId, RefreshToken refreshToken)
        {
            TimeSpan expiration = refreshToken.Expiration - DateTime.Now;

            await _redisCacheService.SetValueAsync(refreshToken.Token, userId.ToString(),  expiration);
        }

        [HttpGet("getusersofcertainrole")]
        public IActionResult GetUsersOfCertainRole(string claimName)
        {
            var result =  _userService.GetUsersOfCertainRole(claimName);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Error fetcing users of a certain role");
        }
    }
}
