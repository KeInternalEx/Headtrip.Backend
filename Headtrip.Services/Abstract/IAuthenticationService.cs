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
        Task<LoginResult> LoginUserByUsername(string username, string password);
        Task<LoginResult> LoginUserByEmail(string email, string password);
        Task<LoginResult> LoginUserByUsernameForGameServer(string username, string password);
        Task<LoginResult> LoginUserByEmailForGameServer(string email, string password);

        
    }
}
