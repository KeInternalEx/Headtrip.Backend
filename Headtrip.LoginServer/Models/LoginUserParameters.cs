using System.ComponentModel.DataAnnotations;

namespace Headtrip.LoginServer.Models
{
    public class LoginUserParameters
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
