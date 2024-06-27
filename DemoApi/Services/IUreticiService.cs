using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IUreticiService
    {
        IEnumerable<UrunUretici> GetUreticiler();
    }
}
