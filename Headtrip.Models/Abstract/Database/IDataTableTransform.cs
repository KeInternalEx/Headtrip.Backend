using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Abstract.Database
{
    /**
     * Interface outlining how an ADatabaseObject should be transformed into a data row.
     */
    public interface IDataTableTransform
    {
        void MapToRow(DataRow Row);
        void MapToColumns(DataColumnCollection Columns);

    }
}
