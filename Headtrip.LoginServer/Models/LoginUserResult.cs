using Headtrip.Authentication.Models;
using Headtrip.Objects.Abstract.Results;

namespace Headtrip.LoginServer.Models
{
    public class LoginUserResult : AControllerResult
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
