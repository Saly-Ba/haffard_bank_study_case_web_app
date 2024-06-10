using Newtonsoft.Json;
using HaffardBankWebApp.Models;
using System.Text;
using System.Text.Json;

namespace HaffardBankWebApp.Services;

public class ApiService {
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<PinModel?> GetPin<PinModel>(long clientId)
    {
        var response = await _httpClient.GetAsync($"/api/Card/pin/{clientId}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PinModel>(content)!;
        }

        return default;
    }

    public async Task<ClientModel?> CreateClient<ClientModel>(ClientModel client){
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Client", client);
            Console.WriteLine($"Error fetching card request{response}");
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ClientModel>(content)!;
            }

            return default;
        }
    }

    public async Task<CardModel?> GetCard<CardModel>(long id)
    {
        var response = await _httpClient.GetAsync($"/api/Card/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CardModel>(content)!;
        }

        return default;
    }

    public async Task<bool> ActivateCard(long id, PinModel pin)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(pin);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(new HttpMethod("PUT"), $"/api/Card/activate/{id}?pin={pin.Pin}");

        var response =  await _httpClient.SendAsync(request);

        // var response =  await _httpClient.PutAsync($"/api/Card/activate/{id}", content);

        Console.WriteLine($"Activate Card : {response}");

        // if (response.IsSuccessStatusCode)
        // {
        //     // Read the response content
        //     string responseBody = await response.Content.ReadAsStringAsync();

        //     // Process the response (e.g., display it)
        //     Console.WriteLine("PUT request succeeded!");
        //     Console.WriteLine("Response Content:");
        //     Console.WriteLine(responseBody);
        // }
        // else
        // {
        //     // Handle the case when the request is not successful
        //     Console.WriteLine($"PUT request failed with status code {response.StatusCode}");
        // }

        return true;

    }

    public async Task<bool> DeactivateCard(long id, PinModel pin)
    {

        var json = System.Text.Json.JsonSerializer.Serialize(pin);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(new HttpMethod("PUT"), $"/api/Card/deactivate/{id}?pin={pin.Pin}");

        var response =  await _httpClient.SendAsync(request);

        // var response =  await _httpClient.PutAsync($"/api/Card/activate/{id}", content);

        return response.IsSuccessStatusCode;
    }

}