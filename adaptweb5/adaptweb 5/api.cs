using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public class Api
{
    private readonly HttpClient _httpClient;

    public Api(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<WeatherResponse<T>> GetAsync<T>(string url)
    {
        return await GetDataAsync<T>(url, HttpMethod.Get);
    }

    public async Task<WeatherResponse<T>> PostAsync<T>(string url, object payload)
    {
        return await GetDataAsync<T>(url, HttpMethod.Post, payload);
    }

    public async Task<WeatherResponse<T>> PostAndSaveAsync<T>(string url, object payload, string fileName)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                string jsonData = JsonSerializer.Serialize(data);
                await File.WriteAllTextAsync(fullPath, jsonData);

                return new WeatherResponse<T>
                {
                    Data = data,
                    HttpStatusCode = response.StatusCode,
                    Message = "Success",
                    ApiDataList = new List<T> { data }
                };
            }
            else
            {
                return new WeatherResponse<T>
                {
                    Data = default,
                    HttpStatusCode = response.StatusCode,
                    Message = $"Failed to get data. Status code: {response.StatusCode}",
                    ApiDataList = new List<T>()
                };
            }
        }
        catch (Exception ex)
        {
            return new WeatherResponse<T>
            {
                Data = default,
                HttpStatusCode = HttpStatusCode.InternalServerError,
                Message = $"An error occurred: {ex.Message}",
                ApiDataList = new List<T>()
            };
        }
    }



    private async Task<WeatherResponse<T>> GetDataAsync<T>(string url, HttpMethod method, object payload = null)
    {
        try
        {
            HttpResponseMessage response;

            if (method == HttpMethod.Get)
            {
                response = await _httpClient.GetAsync(url);
            }
            else if (method == HttpMethod.Post)
            {
                response = await _httpClient.PostAsJsonAsync(url, payload);
            }
            else
            {
                throw new ArgumentException("Invalid HTTP method");
            }

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return new WeatherResponse<T>
                {
                    Data = data,
                    HttpStatusCode = response.StatusCode,
                    Message = "Success",
                    ApiDataList = new List<T> { data }
                };
            }
            else
            {
                return new WeatherResponse<T>
                {
                    Data = default,
                    HttpStatusCode = response.StatusCode,
                    Message = $"Failed to get data. Status code: {response.StatusCode}",
                    ApiDataList = new List<T>()
                };
            }
        }
        catch (Exception ex)
        {
            return new WeatherResponse<T>
            {
                Data = default,
                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                Message = $"An error occurred: {ex.Message}",
                ApiDataList = new List<T>()
            };
        }
    }
}
public class WeatherResponse<T>
{
    public string Message { get; set; }
    public System.Net.HttpStatusCode HttpStatusCode { get; set; }
    public T Data { get; set; }
    public List<T> ApiDataList { get; set; } 
}
