using Headtrip.Authentication.Models;
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
        Task<AuthenticationResult> CreateToken(IdentityUser User);


    }
}
