using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Todo.Services.Gravatar
{
    public interface IGravatarDataFetcher
    {
        /// <returns>Returns null when Gravatar service returns request with Http Error Code</returns>
        Task<GravatarResult> GetGravatarDataFromServiceAsync(string hash);
    }

    public class GravatarDataFetcher : IGravatarDataFetcher
    {
        private readonly IHttpClientFactory _clientFactory;

        public GravatarDataFetcher(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <returns>Returns null when Gravatar service returns request with Http Error Code</returns>
        public async Task<GravatarResult> GetGravatarDataFromServiceAsync(string hash)
        {
            var path = $"{hash}.json";

            var client = _clientFactory.CreateClient(GravatarClientConfig.ClientName);

            client.DefaultRequestHeaders.Add("User-Agent", "Todo-App");

            try
            {
                var response = await client.GetAsync(path);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<GravatarResult>(responseBody);
            }
            catch (Exception e)
            {
                //We can discuss how to handle this error?
                //I assume if we have some problems with gravatar I just return null and higher order method decides what to do with that.
                //Cause we can have problems with Gravatar availability we could use Polly for retry policy.
                return null;
            }
        }
    }
}
