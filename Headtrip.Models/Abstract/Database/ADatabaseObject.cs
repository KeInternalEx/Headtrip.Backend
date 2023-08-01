using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Abstract.Database
{
    /**
     * Default IDataTableTransform is implemented using reflection.
     * Override functionality to increase performance.
     */
    public abstract class ADatabaseObject : IDataTableTransform
    {
        public virtual void MapToRow(DataRow Row)
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if (
                    property.CanRead &&
                    !property.IsSpecialName)
                {
                    Row[property.Name] = property.GetValue(this);
                }
            }
        }

        public virtual void MapToColumns(DataColumnCollection Columns)
        {
            foreach (var property in this.GetType().GetProperties())
            {
                if (
                    property.CanRead &&
                    !property.IsSpecialName)
                {
                    Columns.Add(new DataColumn(property.Name, property.PropertyType));
                }
            }
        }



    }
}
