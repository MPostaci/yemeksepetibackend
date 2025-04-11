using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface IRedisCacheService
    {
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value, TimeSpan expiration);
        Task Clear(string key);
        void ClearAll();
    }
}
