using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Headtrip.Utilities.Interface
{
    public interface IContext<T> : IDisposable
    {
        string SprocPrefix { get; }
        IDbConnection Connection { get; }
        TransactionScope BeginTransaction(Transaction? AmbientTransaction = null);
    }
}
