using Dapper;
using Headtrip.LoginServerContext;
using Headtrip.Models.User;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IContext<HeadtripLoginServerContext> _context;

        public UserRepository(IContext<HeadtripLoginServerContext> context)
        {
            _context = context;
        }


        public async Task CreateUser(User user)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[User_Create]",
                param: user,
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteUser(Guid userId)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[User_Delete]",
                param: new
                {
                    UserId = userId
                },
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateUser(User user)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[User_Update]",
                param: user,
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User> GetUserByUserId(Guid userId)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<User>(
                sql: "[User_GetUserByUserId]",
                param: new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<User>(
                sql: "[User_GetUserByUsername]",
                param: new
                {
                    Username = username
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<User>(
                sql: "[User_GetUserByEmail]",
                param: new
                {
                    Email = email
                },
                commandType: CommandType.StoredProcedure);
        }


        public async Task ConfirmEmail(Guid userId)
        {
            await _context.Connection.ExecuteAsync(
                sql: "[User_ConfirmEmail]",
                param: new
                {
                    UserId = userId
                },
                transaction: _context.Transaction,
                commandType: CommandType.StoredProcedure);

        }
    }
}
