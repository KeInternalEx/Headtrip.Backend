﻿using System.Data.Common;
using System.Data;
using Headtrip.Utilities.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Headtrip.GameServerContext
{
    public class HeadtripGameServerContext : IContext<HeadtripGameServerContext>
    {
        public IDbConnection Connection { get; private set; }
        public IDbTransaction? Transaction { get; private set; }

        public HeadtripGameServerContext(IConfiguration configuration) =>
            Connection = new SqlConnection(configuration.GetConnectionString("GameServerConnectionString"));

        public void BeginTransaction()
        {
            if (Transaction == null)
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();

                Transaction = Connection.BeginTransaction();
            }
        }


        public void CommitTransaction()
        {
            Transaction?.Commit();
            Transaction?.Dispose();
            Transaction = null;
        }

        public void Dispose()
        {
            if (Transaction != null)
                RollbackTransaction();

            Connection?.Close();
            Connection?.Dispose();

            GC.SuppressFinalize(this);
        }

        public void RollbackTransaction()
        {
            Transaction?.Rollback();
            Transaction?.Dispose();
            Transaction = null;
        }
    }
}