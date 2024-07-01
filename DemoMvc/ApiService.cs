using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DemoApi.Models;
using DemoMvc.Models;
using Newtonsoft.Json;
using Markalar = DemoApi.Models.Markalar;
using UrunUretici = DemoApi.Models.UrunUretici;

public class ApiServices
{
    private readonly HttpClient _httpClient;

    public ApiServices()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7271/") 
        };
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /*---------------------- Urunler ------------*/
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

    public async Task<bool> UpdateProductAsync(Urunler urun)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Urun/UpdateProduct", urun);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Urun/DeleteProduct/{id}");
        response.EnsureSuccessStatusCode();
    }

    /*---------------------- Urun Grupları ------------*/
    public async Task<List<UrunGruplari>> GetGrups()
    {
        List<UrunGruplari> gruplar = new List<UrunGruplari>();

        HttpResponseMessage response = await _httpClient.GetAsync("api/Grup/GetGrups");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            gruplar = JsonConvert.DeserializeObject<List<UrunGruplari>>(json);
     
        }
        else
        {
            return new List<UrunGruplari>();
        }

        return gruplar;
    }

    /*---------------------- Markalar ------------*/

    public async Task<List<Markalar>> GetAllMarkalar()
    {
        var response = await _httpClient.GetAsync("api/Marka/GetBrands");

        if (response.IsSuccessStatusCode)
        {
            var json = response.Content.ReadAsStringAsync().Result;
            var markalar = JsonConvert.DeserializeObject<List<Markalar>>(json);
            return markalar;
        }
        else
        {
            return new List<Markalar>();
        }
    }

    /*---------------------- Ureticiler ------------*/

    public async Task<List<UrunUretici>> GetUreticiler()
    {
        var response = await _httpClient.GetAsync("api/Uretici/GetUreticiler");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var ureticiler = JsonConvert.DeserializeObject<List<UrunUretici>>(json);
            return ureticiler;
        }
        else
        {
            return new List<UrunUretici>();
        }
    }

}
