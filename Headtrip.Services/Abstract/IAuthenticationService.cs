using Headtrip.Objects.Authentication.Result;
using Headtrip.Objects.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services.Abstract
{
    public interface IAuthenticationService
    {
        Task<RLoginResult> LoginUserByUsername(string username, string password);
        Task<RLoginResult> LoginUserByEmail(string email, string password);
        Task<RLoginResult> LoginUserByUsernameForGameServer(string username, string password);
        Task<RLoginResult> LoginUserByEmailForGameServer(string email, string password);

        
    }
}
