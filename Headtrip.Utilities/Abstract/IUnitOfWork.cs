using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities.Abstract
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }


    public interface IUnitOfWork<T> : IUnitOfWork { }
    public interface IUnitOfWork<T, U> : IUnitOfWork { }
}
