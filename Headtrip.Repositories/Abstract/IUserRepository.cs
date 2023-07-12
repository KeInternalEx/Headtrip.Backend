using Headtrip.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task DeleteUser(Guid userId);
        Task UpdateUser(User user);



        Task<User> GetUserByUserId(Guid userId);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);


        Task ConfirmEmail(Guid userId);
    }
}
