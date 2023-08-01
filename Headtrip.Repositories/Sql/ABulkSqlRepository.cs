using Dapper;
using Headtrip.Objects.Abstract.Database;
using Headtrip.Repositories.Generic;
using Headtrip.Repositories.Sql.Implementations;
using Headtrip.Utilities.Abstract;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Sql
{
    public abstract class ABulkSqlRepository<TDatabaseObject, TContext> :
        ADapperSqlImplementation<TContext>, ISqlImplementation, IBulkCopy<TDatabaseObject>
        where TDatabaseObject : IDataTableTransform
    {
        protected string _TableName;
        protected string _TempTableName;

        protected int? _BatchSize;

        protected ABulkSqlRepository(
            IContext<TContext> context,
            string tableName,
            int? batchSize) : base(context)
        {
            _TableName = tableName;
            _TempTableName = "#" + tableName;
            _BatchSize = batchSize;
        }



        #region IBulkCopy<TDatabaseObject>
        protected abstract Task<IEnumerable<TDatabaseObject>> FinalizeBulkCopy();
        public async Task<IEnumerable<TDatabaseObject>> BulkCopy(IEnumerable<TDatabaseObject> Objects)
        {
            if (Objects.Count() == 0)
                throw new ArgumentException("No objects in collection");

            var connection = _Context.Connection as SqlConnection;
            var transaction = _Context.Transaction as SqlTransaction;

            if (
                connection == null ||
                transaction == null)
            {
                throw new Exception($"IContext<{typeof(TContext).Name}> is not a SQL based context.");
            }

            var dataTable = await BulkTransform(Objects);
            if (dataTable == null)
                return Objects;

            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.DestinationTableName = _TempTableName;
                bulkCopy.BatchSize = _BatchSize.HasValue ? _BatchSize.Value : bulkCopy.BatchSize;

                await bulkCopy.WriteToServerAsync(dataTable);
            }

            return await FinalizeBulkCopy();
        }

        public Task<DataTable?> BulkTransform(IEnumerable<TDatabaseObject> Objects)
        {
            try
            {
                if (Objects.Count() == 0)
                    throw new ArgumentException("No objects in collection");


                var table = new DataTable();

                Objects.First().MapToColumns(table.Columns);

                foreach (var Object in Objects)
                {
                    Object.MapToRow(table.NewRow());
                }

                return Task.FromResult<DataTable?>(table);
            }
            catch (Exception ex)
            {
                return Task.FromException<DataTable?>(ex);
            }
        }
        #endregion

    }
}
