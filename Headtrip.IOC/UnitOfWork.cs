using System.Data;

namespace Headtrip.IOC
{
    public class UnitOfWork
    {
        private readonly IDbConnection _Connection;
        private readonly IDbTransaction _Transaction;

        public UnitOfWork(
            IDbTransaction transaction,
            IDbConnection connection)
        {
            _Transaction = transaction;
            _Connection = connection;
        }




        public void Commit()
        {

        }
    }




}