using Headtrip.Authentication.Models;
using Headtrip.Models.User;
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
        Task<AuthenticationResult> AuthenticateUserByUsername(string username, string password);
        Task<AuthenticationResult> AuthenticateUserByEmail(string email, string password);
        Task<AuthenticationResult> AuthenticateUserByUsernameForGameServer(string username, string password);
        Task<AuthenticationResult> AuthenticateUserByEmailForGameServer(string email, string password);

        
    }
}
