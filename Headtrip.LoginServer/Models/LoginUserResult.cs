using Headtrip.Authentication.Models;
using Headtrip.Models.Abstract;

namespace Headtrip.LoginServer.Models
{
    public class LoginUserResult : ControllerResult
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
