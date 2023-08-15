using Dapper;
using Headtrip.Objects.Abstract.Database;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Sql.Implementations
{
    public abstract class ADapperSqlImplementation<TContext> : ISqlImplementation
    {
        protected readonly IContext<TContext> _Context;

        protected ADapperSqlImplementation(IContext<TContext> context)
            => _Context = context;

        protected string ToSpName(string Name)
           => $"[{_Context.SprocPrefix}{Name}]";

        protected async Task<TReturn> QuerySingleAsync<TReturn, TParameter>(string ProcedureName, TParameter Parameter)
            => await _Context.Connection.QuerySingleAsync<TReturn>(
                sql: ToSpName(ProcedureName),
                param: Parameter,
                commandType: System.Data.CommandType.StoredProcedure);

        protected async Task<TReturn> QuerySingleAsync<TReturn>(string ProcedureName)
            => await _Context.Connection.QuerySingleAsync<TReturn>(
                sql: ToSpName(ProcedureName),
                commandType: System.Data.CommandType.StoredProcedure);

        protected async Task<IEnumerable<TReturn>> QueryAsync<TReturn, TParameter>(string ProcedureName, TParameter Parameter)
            => await _Context.Connection.QueryAsync<TReturn>(
                sql: ToSpName(ProcedureName),
                param: Parameter,
                commandType: System.Data.CommandType.StoredProcedure);

        protected async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string ProcedureName)
            => await _Context.Connection.QueryAsync<TReturn>(
                sql: ToSpName(ProcedureName),
                commandType: System.Data.CommandType.StoredProcedure);

        protected async Task<int> ExecuteAsync<TParameter>(string ProcedureName, TParameter Parameter)
            => await _Context.Connection.ExecuteAsync(
                sql: ToSpName(ProcedureName),
                param: Parameter,
                commandType: System.Data.CommandType.StoredProcedure);

        protected async Task<int> ExecuteAsync(string ProcedureName)
            => await _Context.Connection.ExecuteAsync(
                sql: ToSpName(ProcedureName),
                commandType: System.Data.CommandType.StoredProcedure);
    }
}
