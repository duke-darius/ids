using System.Security.Claims;
using Blazored.LocalStorage;
using Iceni.Lib.ApiConsumer;
using Microsoft.AspNetCore.Components;
using RestSharp;

namespace Iceni.Admin.Wasm.Services;

public class ApiClient
{
    private readonly ILocalStorageService _localStorage;
    private readonly ISyncLocalStorageService _syncLocalStorageService;
    private readonly IConfiguration _configuration;
    public IceniApiClient Client { get; set; }

    public ApiClient(HttpClient client, ILocalStorageService localStorage, NavigationManager navigationManager, ISyncLocalStorageService syncLocalStorageService, IConfiguration configuration)
    {
        _localStorage = localStorage;
        _syncLocalStorageService = syncLocalStorageService;
        _configuration = configuration;
        Client = new IceniApiClient(new RestClient(client));

        SyncAuthSync();
        
        Client.OnUnauthorisedEventHandler += (sender, args) =>
        {
            Console.WriteLine("TOKEN EXPIRED");
            navigationManager.NavigateTo("/auth/logout");
        };
    }

    public async Task SyncAuth()
    {
        var tokenExists = await _localStorage.ContainKeyAsync("auth-token");
        if (tokenExists)
        {
            Client.JwtToken = await _localStorage.GetItemAsync<string>("auth-token");
        }
    }
    
    public void SyncAuthSync()
    {
        var tokenExists = _syncLocalStorageService.ContainKey("auth-token");
        if (tokenExists)
        {
            Client.JwtToken = _syncLocalStorageService.GetItem<string>("auth-token");
        }
    }

    public bool TokenInvalid
    {
        get
        {
            
            var res = Client.JwtToken == null || Client.Jwt == null ||
                Client.Jwt?.ValidTo.ToUniversalTime() <= DateTime.UtcNow.AddMinutes(10);

            if (_configuration["Debug:AuthLogging"] != "enabled") return res;
            
            
            Console.WriteLine(
                $"TokenNull: {Client.JwtToken == null}, JwtNull: {Client.Jwt == null}, ValidFrom: {Client.Jwt?.ValidFrom.ToUniversalTime()}, ValidTo: {Client.Jwt?.ValidTo.ToUniversalTime()}");
            var now = Client.Jwt?.ValidTo.ToUniversalTime();
            Console.WriteLine($"valid till: {now}, Current time: {DateTime.UtcNow.AddMinutes(10)}");
            Console.WriteLine($"Invalid: {res}");

            return res;
        }
    }

    public async Task SetToken(string jwtToken)
    {
        Client.JwtToken = jwtToken;
        await _localStorage.SetItemAsync("auth-token", jwtToken);
    }

    public async Task Logout()
    {
        Client.JwtToken = null;
        await _localStorage.RemoveItemAsync("auth-token");
    }

    public string GetName()
    {
        return Client.Identity?.Name ?? string.Empty;
    }

    public string GetEmail()
    {
        return Client.Identity?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }
}