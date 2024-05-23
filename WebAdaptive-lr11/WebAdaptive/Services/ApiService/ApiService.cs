using Microsoft.Extensions.Configuration;

namespace WebAdaptive.Services.ApiService
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _client = httpClient;
            _apiKey = configuration.GetValue<string>("ApiKey") ?? throw new ArgumentNullException("ApiKey");
        }

        public async Task<string> GetCorrectAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text), "Text cannot be null or empty");

            var requestUrl = $"https://api.textgears.com/correct?text={text}&language=en-GB&key={_apiKey}";

            var response = await _client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode(); 
            return await response.Content.ReadAsStringAsync();
        }
    }
}
