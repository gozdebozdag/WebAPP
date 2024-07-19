using DemoApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DemoApi.Services
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User> Authenticate(string username, string password)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var user = await con.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username });

                if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
                    return null;

                return user;
            }
        }

        public async Task<User> Register(string username, string email, string password)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (await con.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username }) != null)
                {
                    throw new ApplicationException("Username \"" + username + "\" is already taken");
                }

                byte[] passwordHash;
                CreatePasswordHash(password, out passwordHash);

                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = Convert.ToBase64String(passwordHash),
                    CreatedAt = DateTime.UtcNow
                };

                await con.ExecuteAsync("INSERT INTO Users (Username, Email, PasswordHash, CreatedAt) VALUES (@Username, @Email, @PasswordHash, @CreatedAt)", user);

                return user;
            }
        }

        public async Task<User> GetById(int id)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                return await con.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            }
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            var passwordHash = Convert.FromBase64String(storedHash);
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
