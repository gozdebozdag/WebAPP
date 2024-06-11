using DemoApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DemoApi.Services
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString;

        public ProductService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Dictionary<string, string>> GetUrunlerWithMarka()
        {
            var urunlerWithMarka = new List<Dictionary<string, string>>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT u.UrunAdi, m.Marka FROM Urunler u INNER JOIN Markalar m ON m.MarkaId = u.MarkaId";
                var command = new SqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new Dictionary<string, string>
                    {
                        { "UrunAdi", reader["UrunAdi"].ToString() },
                        { "Marka", reader["Marka"].ToString() }
                    };
                        urunlerWithMarka.Add(item);
                    }
                }
            }

            return urunlerWithMarka;
        }

        public IEnumerable<Urunler> GetAllProducts()
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<Urunler>("SELECT * FROM Urunler").ToList();
            }
        }

        public Urunler GetProductById(int id)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.QueryFirstOrDefault<Urunler>("SELECT * FROM Urunler WHERE UrunId = @Id", new { Id = id });
            }
        }

        public void AddProduct(Urunler product)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Urunler (MarkaId, GrupId, UreticiId, UrunKodu, UrunAdi, Kdv, AktifMi) 
                              VALUES (@MarkaId, @GrupId, @UreticiId, @UrunKodu, @UrunAdi, @Kdv, @AktifMi)";
                con.Execute(query, product);
            }
        }

        public void UpdateProduct(Urunler product)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Urunler SET MarkaId = @MarkaId, GrupId = @GrupId, UreticiId = @UreticiId, 
                              UrunKodu = @UrunKodu, UrunAdi = @UrunAdi, Kdv = @Kdv, AktifMi = @AktifMi 
                              WHERE UrunId = @UrunId";
                con.Execute(query, product);
            }
        }

        public void DeleteProduct(int id)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                con.Execute("DELETE FROM Urunler WHERE UrunId = @Id", new { Id = id });
            }
        }
    }
}
