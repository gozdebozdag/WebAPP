using DemoApi.Context;
using DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DemoDbContext _context;

        public AuthController(IConfiguration configuration, DemoDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            var existingUserByEmail = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUserByEmail != null)
            {
                return Conflict("Email already exists.");
            }

            if (!IsValidPassword(request.Password))
            {
                return BadRequest("Password must be at least 8 characters long and include at least one digit and one uppercase letter.");
            }
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password 
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string Username,string Password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (user.PasswordHash != Password)
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);
            return Ok(new { Username = user.Username, Token = token });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim (ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["AppSettings:Issuer"],
                audience: _configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }

            bool hasDigit = password.Any(char.IsDigit);
            bool hasUpperCase = password.Any(char.IsUpper);

            return hasDigit && hasUpperCase;
        }
    }
}
