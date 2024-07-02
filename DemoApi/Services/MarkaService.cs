﻿using Microsoft.AspNetCore.Mvc;
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
        public void AddBrand(Markalar marka)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Markalar (MarkaId, Marka) 
                              VALUES (@MarkaId, @Marka)";
                con.Execute(query, marka);
            }
        }

        public void DeleteBrand(int id)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                con.Execute("DELETE FROM Markalar WHERE MarkaId = @Id", new { Id = id });
            }
        }

        public IEnumerable<Markalar> GetAllBrands()
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<Markalar>("SELECT * FROM Markalar").ToList();
            }
        }

        public Markalar GetBrand(Markalar marka)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.QueryFirstOrDefault<Markalar>("SELECT * FROM Markalar WHERE Marka = @Marka", new { Marka = marka.Marka });
            }
        }

        public Markalar GetBrandById(int id)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return con.QueryFirstOrDefault<Markalar>("SELECT * FROM Markalar WHERE UrunId = @Id", new { Id = id });
            }
        }

        public void UpdateBrand(Markalar marka)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Markalar SET MarkaId = @MarkaId, Marka = @Marka
                              WHERE MarkaId = @MarkaId";
                con.Execute(query, marka);
            }
        }
    }
}
