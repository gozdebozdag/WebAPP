using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IMarkaService
    {
        IEnumerable<Markalar> GetAllMarkalar();
        Markalar GetBrands(Markalar marka);
        Markalar GetBrandById(int id);
        void AddBrand(Markalar marka);
        void UpdateBrand(Markalar marka);
        void DeleteBrand(int id);
    }
}
