using DemoApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace DemoApi.Services
{
    public class GrupService : IGrupService
    {
        private readonly string _connectionString;

        public GrupService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<UrunGruplari> GetGrups()
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<UrunGruplari>("SELECT * FROM UrunGruplari").ToList();
            }
        }

        
    }
}
