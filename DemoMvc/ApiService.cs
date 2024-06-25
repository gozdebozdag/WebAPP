using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DemoApi.Models;
using DemoMvc.Models;
using Newtonsoft.Json;

public class ApiServices
{
    private readonly HttpClient _httpClient;

    public ApiServices()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7271/") // API'nin adresi ve portu buraya yazılmalı
        };
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<List<Urunler>> GetProducts()
    {
        List<Urunler> products = new List<Urunler>();

        HttpResponseMessage response = await _httpClient.GetAsync("api/Urun/GetProducts");
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            products = JsonConvert.DeserializeObject<List<Urunler>>(jsonString);
        }
        return products;
    }

    public async Task<Urunler> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Urun/GetProduct/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Urunler>(content);
    }

    public async Task AddProductAsync(Urunler urun)
    {
        var content = new StringContent(JsonConvert.SerializeObject(urun), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Urun/AddProduct", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateProductAsync(Urunler urun)
    {
        var content = new StringContent(JsonConvert.SerializeObject(urun), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("api/Urun/UpdateProduct", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Urun/DeleteProduct/{id}");
        response.EnsureSuccessStatusCode();
    }

    
}
