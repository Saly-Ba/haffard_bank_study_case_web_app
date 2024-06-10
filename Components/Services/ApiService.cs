using Newtonsoft.Json;
using HaffardBankWebApp.Models;

namespace HaffardBankWebApp.Services;

public class ApiService {
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> Get<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content)!;
        }

        return default;
    }

    public async Task<IEnumerable<T>> GetPin<T>(int clientId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<T>>($"card/pin/{clientId}");
        }

}