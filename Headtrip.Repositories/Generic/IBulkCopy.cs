using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Generic
{
    public interface IBulkCopy<TDatabaseObject> where TDatabaseObject : IDataTableTransform
    {
        Task<IEnumerable<TDatabaseObject>> BulkCopy(IEnumerable<TDatabaseObject> Objects);
        Task<DataTable?> BulkTransform(IEnumerable<TDatabaseObject> Objects);
    }
}
