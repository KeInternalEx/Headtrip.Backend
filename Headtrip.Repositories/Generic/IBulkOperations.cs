using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Generic
{
    public interface IBulkOperations<TDatabaseObject> where TDatabaseObject : IDataTableTransform
    {
        Task<IEnumerable<TDatabaseObject>?> BulkCopy(IEnumerable<TDatabaseObject> Objects, bool AutoFlush = true);
        Task<IEnumerable<TDatabaseObject>> BulkCopyFlush();

        Task<IEnumerable<TDatabaseObject>?> BulkUpdate(IEnumerable<TDatabaseObject> Objects, bool AutoFlush = true);
        Task<IEnumerable<TDatabaseObject>> BulkUpdateFlush();

        Task<DataTable?> BulkTransform(IEnumerable<TDatabaseObject> Objects);
    }
}
