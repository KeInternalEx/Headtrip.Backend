using Dapper;
using Headtrip.Objects.Abstract.Database;
using Headtrip.Repositories.Sql.Implementations;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Sql
{
    public abstract class ASqlRepository<TDatabaseObject, TContext> : ADapperSqlImplementation<TContext>, ISqlImplementation
        where TDatabaseObject : ADatabaseObject
    {
        protected string _TableName;

        protected ASqlRepository(IContext<TContext> context, string tableName) : base(context)
        {
            _TableName = tableName;
        }



    }
}
