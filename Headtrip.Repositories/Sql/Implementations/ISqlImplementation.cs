using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Sql.Implementations
{
    public interface ISqlImplementation
    {
        protected string ToSpName(string Name)
           => throw new NotImplementedException(nameof(ToSpName));

        protected Task<TReturn> QuerySingleAsync<TReturn, TParameter>(string ProcedureName, TParameter Parameter)
            => throw new NotImplementedException(nameof(QuerySingleAsync));

        protected Task<TReturn> QuerySingleAsync<TReturn>(string ProcedureName)
            => throw new NotImplementedException(nameof(QuerySingleAsync));

        protected Task<IEnumerable<TReturn>> QueryAsync<TReturn, TParameter>(string ProcedureName, TParameter Parameter)
            => throw new NotImplementedException(nameof(QueryAsync));

        protected Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string ProcedureName)
            => throw new NotImplementedException(nameof(QueryAsync));

        protected Task<int> ExecuteAsync<TParameter>(string ProcedureName, TParameter Parameter)
            => throw new NotImplementedException(nameof(ExecuteAsync));

        protected Task<int> ExecuteAsync(string ProcedureName)
            => throw new NotImplementedException(nameof(ExecuteAsync));
    }
}
