using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.SQLUtilities
{
    public class SqlScriptExpandedObject
    {
        public Dictionary<string, SqlColumn> _columnAttributes = new Dictionary<string, SqlColumn>();
        public Dictionary<string, Tuple<Type, PropertyInfo>> _foreignKeyMappings = new Dictionary<string, Tuple<Type, PropertyInfo>>();
        public SqlTable? _tableAttribute;
        public Type _type;

        public SqlScriptExpandedObject(Type objectType)
        {
            _type = objectType;
            _tableAttribute = objectType.GetCustomAttribute<SqlTable>();

            if (_tableAttribute == null)
                throw new Exception($"{objectType.Name} is not a SQL Table Entry");

            var properties = objectType.GetProperties();
            foreach (var propInfo in properties)
            {
                var propertyAttribute = propInfo.GetCustomAttribute<SqlColumn>();
                if (propertyAttribute == null)
                    continue;

                _columnAttributes.Add(propInfo.Name, propertyAttribute);
            }
        }


        public void AddForeignKeyMapping(string mapping, Tuple<Type, PropertyInfo> propInfo) =>
            _foreignKeyMappings.Add(mapping, propInfo);

    }
}
