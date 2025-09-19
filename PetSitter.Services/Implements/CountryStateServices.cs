using Microsoft.Extensions.Configuration;

namespace PetSitter.Services.Implements;

public class CountryStateServices
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;
    
    public CountryStateServices(HttpClient client, IConfiguration configuration)
    {
        _httpClient = client;
        _apiKey = configuration["CSCKey:Api"];
    }
    
    public async Task<string> GetAllCountries()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.countrystatecity.in/v1/countries"),
            Headers =
            {
                { "X-CSCAPI-KEY", _apiKey },
            }
        };
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
    
    public async Task<string> GetStatesByCountry(string countryCode)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://api.countrystatecity.in/v1/countries/{countryCode}/states"),
            Headers =
            {
                { "X-CSCAPI-KEY", _apiKey },
            }
        };
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }
}