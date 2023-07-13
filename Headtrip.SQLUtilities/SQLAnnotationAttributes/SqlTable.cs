using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.SQLUtilities.SQLAnnotationAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SqlTable : System.Attribute
    {
        public string _sqlTableName;
        public string _context;

        public SqlTable(string sqlTableName, string context)
        {
            _sqlTableName = sqlTableName;
            _context = context;
        }
    }
}
