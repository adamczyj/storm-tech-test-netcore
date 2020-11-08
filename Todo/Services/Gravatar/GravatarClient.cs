using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Todo.Services.Gravatar
{
    public interface IGravatarClient
    {
        Task<string> GetUserNameAsync(string email);
        string GetHash(string emailAddress);
    }

    public class GravatarClient : IGravatarClient
    {
        private readonly IGravatarDataFetcher _dataFetcher;
        private readonly IMemoryCache _cache;
        private const int ClientCacheInMinutes = 1;

        public GravatarClient(IGravatarDataFetcher dataFetcher, IMemoryCache cache)
        {
            _cache = cache;
            _dataFetcher = dataFetcher;
        }

        public async Task<string> GetUserNameAsync(string email)
        {
            var gravatarData = await GetGravatarDataAsync(email);

            //If we got some problems with gravatar just return null
            if (gravatarData == null)
            {
                return null;
            }

            return gravatarData.GetDisplayName();
        }

        public string GetHash(string emailAddress)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(emailAddress.Trim().ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("X2"));
                }

                return builder.ToString().ToLowerInvariant();
            }
        }

        private async Task<GravatarResult> GetGravatarDataAsync(string email)
        {
            return await _cache.GetOrCreateAsync($"GravatarResult_CacheKey_{email}", async entry =>
            {
                entry.SetOptions(new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(ClientCacheInMinutes)
                });

                return await _dataFetcher.GetGravatarDataFromServiceAsync(GetHash(email));
            });
        }
    }
}