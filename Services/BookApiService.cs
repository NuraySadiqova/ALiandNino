
using AliAndNinoClone.Models;
using System.Net.Http.Json;



namespace AliAndNinoClone.Services
{


    public class BookApiService
    {
        private readonly HttpClient _httpClient;

        public BookApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GoogleBookResponse> SearchBooksAsync(string query)
        {
            // Google-a deyirik ki, bizə bu mövzuda kitabları tap
            var url = $"https://www.googleapis.com/books/v1/volumes?q={query}&maxResults=20";
            return await _httpClient.GetFromJsonAsync<GoogleBookResponse>(url);
        }
    }

}
