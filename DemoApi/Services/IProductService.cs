using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IProductService
    {
        IEnumerable<Urunler> GetAllProducts();
        Urunler GetProductById(int id);
        void AddProduct(Urunler product);
        void UpdateProduct(Urunler product);
        void DeleteProduct(int id);
    }
}
