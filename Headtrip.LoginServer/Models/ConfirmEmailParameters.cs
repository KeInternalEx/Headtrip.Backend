using System.ComponentModel.DataAnnotations;

namespace Headtrip.LoginServer.Models
{
    public class ConfirmEmailParameters
    {
        [Required]
        public string? Parameter { get; set; }
    }
}
