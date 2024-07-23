using System.ComponentModel.DataAnnotations;

namespace DemoApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }=string.Empty;
        public string Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        //public DateTime CreatedAt { get; set; }
    }
}
