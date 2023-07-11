using Headtrip.Users.Models;
using Headtrip.Utilities.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Users
{

    public class UserStore : IUserStore<User>
    {
        private IContext _context;
        private IUnitOfWork _unitOfWork;

        

        public UserStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString
        }
    }
}
