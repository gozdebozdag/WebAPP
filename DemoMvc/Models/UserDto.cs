using System.ComponentModel.DataAnnotations;

namespace DemoMvc.Models
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
