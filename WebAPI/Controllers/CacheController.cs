using Core.CrossCuttingConcerns.Caching;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;


        public CacheController (IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        [HttpGet("getviakey")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await _redisCacheService.GetValueAsync(key));
        }

        [HttpPost("set")]
        public async Task<IActionResult> Set([FromBody] RedisCacheRequestModel redisCacheRequestModel, TimeSpan expiration)
        {
            await _redisCacheService.SetValueAsync(redisCacheRequestModel.Key, redisCacheRequestModel.Value, expiration);
            return Ok();
        }

        [HttpPost("deleteviakey")]
        public async Task<IActionResult> Delete(string key)
        {
            await _redisCacheService.Clear(key);
            return Ok();
        }
    }
}
