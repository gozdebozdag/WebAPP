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
using UserDto = DemoMvc.Models.UserDto;

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
    public async Task<UrunGruplari> GetGrupByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Grup/GetGrupById/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UrunGruplari>(content);
    }

    public async Task AddGrupAsync(UrunGruplari grup)
    {
        var content = new StringContent(JsonConvert.SerializeObject(grup), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Grup/AddGrup", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> UpdateGrupAsync(UrunGruplari grup)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Grup/UpdateGrup", grup);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteGrupAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Grup/DeleteGrup/{id}");
        response.EnsureSuccessStatusCode();
    }

    /*---------------------- Markalar ------------*/

    public async Task<List<Markalar>> GetBrand()
    {
        List<Markalar> brands = new List<Markalar>();

        HttpResponseMessage response = await _httpClient.GetAsync("api/Marka/GetBrand");
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            brands = JsonConvert.DeserializeObject<List<Markalar>>(jsonString);
        }
        return brands;
    }

    public async Task<Markalar> GetBrandByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Marka/GetBrand/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Markalar>(content);
    }

    public async Task AddBrandAsync(Markalar marka)
    {
        var content = new StringContent(JsonConvert.SerializeObject(marka), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Marka/AddBrand", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> UpdateBrandAsync(Markalar marka)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Marka/UpdateBrand", marka);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteBrandAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Marka/DeleteBrand/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<Markalar>> GetAllMarkalar()
    {
        List<Markalar> brands = new List<Markalar>();

        HttpResponseMessage response = await _httpClient.GetAsync("api/Marka/GetBrand");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            brands = JsonConvert.DeserializeObject<List<Markalar>>(json);
        }
        else
        {
            return new List<Markalar>();
        }

        return brands;
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

    public async Task<UrunUretici> GetUreticiByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/Uretici/GetUreticiler/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UrunUretici>(content);
    }

    public async Task AddUreticiAsync(UrunUretici uretici)
    {
        var content = new StringContent(JsonConvert.SerializeObject(uretici), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Uretici/AddUretici", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<bool> UpdateUreticiAsync(UrunUretici uretici)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Uretici/UpdateUretici", uretici);
        return response.IsSuccessStatusCode;
    }

    public async Task DeleteUreticiAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/Uretici/DeleteUretici/{id}");
        response.EnsureSuccessStatusCode();
    }

    /*---------------------- Auth ------------*/
    public async Task<DemoMvc.Models.User> RegisterAsync(UserDto userDto)
    {
        var jsonContent = JsonConvert.SerializeObject(userDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/auth/register", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DemoMvc.Models.User>(responseData);
        }
        return null;
    }

    public async Task<string> LoginAsync(UserDto userDto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"api/Auth/login", content);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadAsStringAsync();
        return token;
    }
}
