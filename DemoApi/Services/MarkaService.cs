using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using DemoApi.Models;
using Dapper;

namespace DemoApi.Services
{
    public class MarkaService : IMarkaService
    {
        private readonly string _connectionString;

        public MarkaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        IEnumerable<Markalar> IMarkaService.GetAllMarkalar()
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<Markalar>("SELECT * FROM Markalar").ToList();
            }
        }
    }
    
    
}
