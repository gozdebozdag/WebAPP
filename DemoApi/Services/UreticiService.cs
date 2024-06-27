using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace DemoApi.Services
{
    public class UreticiService : IUreticiService
    {
        private readonly string _connectionString;

        public UreticiService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IEnumerable<UrunUretici> GetUreticiler()
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<UrunUretici>("SELECT * FROM UrunUretici").ToList();
            }
        }
    }
}
