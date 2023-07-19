using Headtrip.Utilities.Abstract;
using System.Data;

namespace Headtrip.Utilities
{
    public class UnitOfWork<T> : IUnitOfWork<T>
    {
        private readonly IContext<T> _Context;

        public UnitOfWork(IContext<T> Context) => _Context = Context;

        public void BeginTransaction() => _Context.BeginTransaction();
        public void CommitTransaction() => _Context.CommitTransaction();
        public void Dispose() => _Context.Dispose();
        public void RollbackTransaction() => _Context.RollbackTransaction();
        public void Finalize(bool success)
        {
            if (success)
                CommitTransaction();
            else
                RollbackTransaction();
        }
    }


    public class UnitOfWork<T, U> : IUnitOfWork<T, U>
    {
        private readonly IContext<T> _ContextA;
        private readonly IContext<U> _ContextB;

        public UnitOfWork(
            IContext<T> ContextA,
            IContext<U> ContextB)
        {
            _ContextA = ContextA;
            _ContextB = ContextB;
        }

        public void BeginTransaction()
        {
            if (_ContextA.Transaction == null &&
                _ContextB.Transaction == null)
            {
                _ContextA.BeginTransaction();
                _ContextB.BeginTransaction();
            }

        }

        public void CommitTransaction()
        {
            if (_ContextA.Connection.State == ConnectionState.Open &&
                _ContextB.Connection.State == ConnectionState.Open)
            {
                if (_ContextA.Transaction != null &&
                    _ContextB.Transaction != null)
                {
                    _ContextA.CommitTransaction();
                    _ContextB.CommitTransaction();
                }
                else
                    throw new Exception("One or more transactions were not in an open state, unable to commit transaction.");

            }
            else
                throw new Exception("One or more sockets were closed, unable to commit transaction.");
        }

        public void Dispose()
        {
            _ContextA.Dispose();
            _ContextB.Dispose();
        }

        public void RollbackTransaction()
        {
            if (_ContextA.Connection.State == ConnectionState.Open &&
                _ContextB.Connection.State == ConnectionState.Open)
            {
                if (_ContextA.Transaction != null &&
                    _ContextB.Transaction != null)
                {
                    _ContextA.RollbackTransaction();
                    _ContextB.RollbackTransaction();
                }
                else
                    throw new Exception("One or more transactions were not in an open state, unable to roll back transaction.");
            }
            else
                throw new Exception("One or more sockets were closed, unable to roll back transaction.");
        }


        public void Finalize(bool success)
        {
            if (success)
                CommitTransaction();
            else
                RollbackTransaction();
        }



    }




}