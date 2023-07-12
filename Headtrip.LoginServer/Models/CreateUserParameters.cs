using System.ComponentModel.DataAnnotations;

namespace Headtrip.LoginServer.Models
{
    public class CreateUserParameters
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
