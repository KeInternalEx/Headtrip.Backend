using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities.Interface
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begins a new transaction. Only one may exist at a time.
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        void CommitTransaction();
        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Commits the transaction if successful, rolls it back if unsuccessful
        /// </summary>
        /// <param name="Success">Whether or not the wrapped operations were successful.</param>
        void Finalize(bool Success);
    }


    public interface IUnitOfWork<T> : IUnitOfWork { }
    public interface IUnitOfWork<T, U> : IUnitOfWork { }
}
