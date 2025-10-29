using Blazored.LocalStorage;
using System.Net.Http.Json;
using Wallet.Shared.Models;

namespace Wallet.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;

        public AuthService(HttpClient http, ILocalStorageService localStorageService)
        {
            _http = http;
            _localStorageService = localStorageService;
        }

        public async Task<ServiceResponse<string>> Login(UserLogin user)
        {
            var result = await _http.PostAsJsonAsync("api/authorization/login", user);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(UserRegister user)
        {
            var result = await _http.PostAsJsonAsync("api/authorization/register", user);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

    }
}
