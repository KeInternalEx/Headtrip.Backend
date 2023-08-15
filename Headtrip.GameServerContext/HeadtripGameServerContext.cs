using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Headtrip.Utilities.Interface;
using System.Transactions;

namespace Headtrip.GameServerContext
{
    public sealed class HeadtripGameServerContext : IContext<HeadtripGameServerContext>
    {
        private readonly SqlConnection _Connection;

        public IDbConnection Connection { get { return _Connection; } }
        public string SprocPrefix { get; private set; } = "gs";


        public HeadtripGameServerContext(IConfiguration configuration)
            => _Connection = new SqlConnection(configuration.GetConnectionString("GameServerConnectionString"));

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