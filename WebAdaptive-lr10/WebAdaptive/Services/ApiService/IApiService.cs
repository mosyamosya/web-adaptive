namespace WebAdaptive.Services.ApiService
{
    public interface IApiService
    {
        Task<string> GetCorrectAsync(string url);
    }
}
