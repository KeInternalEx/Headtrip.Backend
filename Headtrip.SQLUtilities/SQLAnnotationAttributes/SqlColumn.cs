using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.SQLUtilities.SQLAnnotationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlColumn : System.Attribute
    {
        public string? type = null; // Required
        public object? defaultValue = default(object?); // Optional

        public bool nullable = false; // False by default
        public bool primaryKey = false; // False by default
        public bool unique = false; // False by default
        public string? index = null; // Index name, optional. Automatically converted to lowercase and prefixed with idx_

        public string? foreignKey = null; // Optional "ClassName.PropertyName"

    }
}
