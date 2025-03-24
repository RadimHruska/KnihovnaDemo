using KnihovnaDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KnihovnaDemo.Functions
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        // API pro výpůjčky
        public async Task<ObservableCollection<Lend>> GetLendsAsync()
        {
            var response = await _httpClient.GetAsync("api/lends");
            response.EnsureSuccessStatusCode();
            var lends = await response.Content.ReadFromJsonAsync<List<Lend>>();
            return new ObservableCollection<Lend>(lends);
        }

        public async Task<int> ReturnBookAsync(int id, DateTime returned)
        {
            var data = new { Id = id, ReturnedDate = returned };
            var response = await _httpClient.PutAsJsonAsync($"api/lends/{id}/return", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task<int> CreateLendAsync(int idUser, int idBook, DateTime lendedTime)
        {
            var data = new { IdUser = idUser, IdBook = idBook, LandedDate = lendedTime };
            var response = await _httpClient.PostAsJsonAsync("api/lends", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        // API pro uživatele
        public async Task<ObservableCollection<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/users");
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            return new ObservableCollection<User>(users);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/users/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<int> CreateUserAsync(string name, string password, bool isAdmin)
        {
            var data = new { Name = name, Password = password, IsAdmin = isAdmin };
            var response = await _httpClient.PostAsJsonAsync("api/users", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateUserAsync(int id, string name, bool isAdmin)
        {
            var data = new { Id = id, Name = name, IsAdmin = isAdmin };
            var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", data);
            response.EnsureSuccessStatusCode();
        }

        // API pro knihy
        public async Task<ObservableCollection<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetAsync("api/books");
            response.EnsureSuccessStatusCode();
            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            return new ObservableCollection<Book>(books);
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/books/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Book>();
        }

        public async Task<int> CreateBookAsync(string name, string author, int inStock)
        {
            var data = new { Name = name, Author = author, InStock = inStock };
            var response = await _httpClient.PostAsJsonAsync("api/books", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateBookAsync(int id, string name, string author, int inStock)
        {
            var data = new { Id = id, Name = name, Author = author, InStock = inStock };
            var response = await _httpClient.PutAsJsonAsync($"api/books/{id}", data);
            response.EnsureSuccessStatusCode();
        }

        // API pro přihlášení
        public async Task<User> LoginAsync(string username, string password)
        {
            var data = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", data);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<User>();
            }
            
            return null;
        }
    }
} 