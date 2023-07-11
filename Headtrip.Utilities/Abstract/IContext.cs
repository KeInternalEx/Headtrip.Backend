using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities.Abstract
{
    public interface IContext<T> : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction? Transaction { get; }

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
