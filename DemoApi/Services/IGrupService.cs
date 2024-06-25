using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IGrupService
    {
        IEnumerable<UrunGruplari> GetGrups();
    }
}
