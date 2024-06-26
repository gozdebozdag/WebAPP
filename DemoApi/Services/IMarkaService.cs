using DemoApi.Models;

namespace DemoApi.Services
{
    public interface IMarkaService
    {
        IEnumerable<Markalar> GetAllMarkalar();
        
    }
}
