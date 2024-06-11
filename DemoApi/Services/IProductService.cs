using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IProductService
    {
        IEnumerable<Urunler> GetAllProducts();
        List<Dictionary<string, string>> GetUrunlerWithMarka();
        Urunler GetProductById(int id);
        void AddProduct(Urunler product);
        void UpdateProduct(Urunler product);
        void DeleteProduct(int id);
    }
}
