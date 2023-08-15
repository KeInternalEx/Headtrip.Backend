using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Headtrip.Utilities.Interface;
using System.Transactions;

namespace Headtrip.LoginServerContext
{
    public sealed class HeadtripLoginServerContext : IContext<HeadtripLoginServerContext>
    {
        private readonly SqlConnection _Connection;

        public IDbConnection Connection { get { return _Connection; } }
        public string SprocPrefix { get; private set; } = "ls";


        public HeadtripLoginServerContext(IConfiguration configuration)
            => _Connection = new SqlConnection(configuration.GetConnectionString("LoginServerConnectionString"));

        public TransactionScope BeginTransaction(Transaction? AmbientTransaction = null)
        {
            var transaction = new TransactionScope();

            if (_Connection.State == ConnectionState.Open)
                _Connection.EnlistTransaction(AmbientTransaction ?? Transaction.Current);

            return transaction;
        }


        public void Dispose()
        {
            if (_Connection != null)
            {
                _Connection.Close();
                _Connection.Dispose();
            }


            GC.SuppressFinalize(this);
        }

    }
}