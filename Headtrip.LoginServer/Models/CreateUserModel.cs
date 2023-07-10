using System.ComponentModel.DataAnnotations;

namespace Headtrip.LoginServer.Models
{
    public class CreateUserModel
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
    }
}
