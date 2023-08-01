using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Generic
{
    /**
     * Generic repository that implements CRUD for a table
     * T = Object
     * U = PrimaryKey type
     */
    public interface IGenericRepository<TDatabaseObject, TPrimaryKey> where TDatabaseObject : ADatabaseObject
    {
        Task<TDatabaseObject> Create(TDatabaseObject Object);

        Task<TDatabaseObject> Read(TPrimaryKey ObjectId);
        Task<IEnumerable<TDatabaseObject>> ReadAll();

        Task<TDatabaseObject> Update(TDatabaseObject Object);
        Task<TDatabaseObject> Delete(TPrimaryKey ObjectId);
    }
}
