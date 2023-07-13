using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.SQLUtilities
{
    public class SqlCreationScriptWriter<T>
    {
        public Dictionary<string, SqlScriptExpandedObject> _expandedTableObjects = new Dictionary<string, SqlScriptExpandedObject>();
        public StringBuilder _stringBuilder;

        public SqlCreationScriptWriter(string context, bool dropExisting)
        {

            var baseType = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && !baseType.IsEquivalentTo(p));

            foreach (var type in types)
            {

                var expandedObject = new SqlScriptExpandedObject(type);
                if (expandedObject._tableAttribute == null ||
                    expandedObject._tableAttribute._sqlTableName == null ||
                    expandedObject._tableAttribute._context != context)
                    continue;


                _expandedTableObjects.Add(type.Name, expandedObject);

            }

            foreach (var obj in _expandedTableObjects)
            {
                foreach (var attr in obj.Value._columnAttributes)
                {
                    if (string.IsNullOrWhiteSpace(attr.Value.foreignKey))
                        continue;

                    var str = attr.Value.foreignKey.Split('.');
                    if (str.Length == 2)
                    {
                        var className = str[0];
                        var propertyName = str[1];

                        if (_expandedTableObjects.TryGetValue(className, out var tableObj))
                        {
                            var t = _expandedTableObjects[className]._type;
                            var propInfo = t.GetProperty(propertyName);
                            if (propInfo == null)
                                throw new Exception($"Encountered missing property info for foreign key {attr.Key} -> {str}");

                            obj.Value.AddForeignKeyMapping(attr.Value.foreignKey, new Tuple<Type, PropertyInfo>(t, propInfo));
                        }
                    }
                    else
                        throw new Exception($"Encountered malformed foreign key entry on {obj.Value._type.Name}.{attr.Key} -> {attr.Value.foreignKey}");
                }   
            }




            _stringBuilder = new StringBuilder();
            if (dropExisting)
            {
                _stringBuilder.Append("DECLARE @sql NVARCHAR(max)=''\r\n\r\nSELECT @sql += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '\r\nFROM   INFORMATION_SCHEMA.TABLES\r\nWHERE  TABLE_TYPE = 'BASE TABLE'\r\n\r\nExec Sp_executesql @sql\r\n");
                _stringBuilder.AppendLine("GO");

                //foreach (var s in droppableForeignKeys)
                //    _stringBuilder.AppendLine(s);

                // foreach (var s in droppablePrimaryKeys)
                //    _stringBuilder.AppendLine(s);

                //_stringBuilder.AppendLine($"DROP TABLE IF EXISTS {obj.Value._tableAttribute?._sqlTableName}");
            }

            foreach (var obj in _expandedTableObjects)
            {
                var hasPrimaryKey = false;
                var columns = string.Empty;
                var indexCreationEntries = new List<string>();
                var droppableForeignKeys = new List<string>();
                var droppablePrimaryKeys = new List<string>();

                foreach (var column in obj.Value._columnAttributes)
                {
                    var columnEntry = $"\t{column.Key} {column.Value.type}";


                    if (column.Value.primaryKey && column.Value.nullable)
                        throw new Exception($"Encountered nullable primary key for entry {obj.Key}.{column.Key}");

                    if (column.Value.primaryKey && column.Value.unique)
                        column.Value.unique = false;

                    if (column.Value.primaryKey && hasPrimaryKey)
                        throw new Exception($"Encountered duplicate primary key, offending entry is {obj.Key}.{column.Key}");


                    if (!string.IsNullOrWhiteSpace(column.Value.foreignKey))
                    {
                        var propInfo = obj.Value._foreignKeyMappings[column.Value.foreignKey];
                        if (propInfo == null)
                        {
                            throw new Exception($"Got bad foreign key mapping {obj.Key}.{column.Key} {column.Value.foreignKey}");
                        }

                        var tblName = _expandedTableObjects[propInfo.Item1.Name]._tableAttribute?._sqlTableName;
                        if (tblName == null)
                        {
                            throw new Exception($"Got null table name for class {propInfo.Item1.Name}");
                        }

                        var columnName = propInfo.Item2.Name;

                        // if (dropExisting)
                        //    droppablePrimaryKeys.Add($"IF OBJECT_ID(N'{obj.Value._tableAttribute?._sqlTableName}', N'U') IS NOT NULL\nBEGIN\nALTER TABLE {obj.Value._tableAttribute?._sqlTableName} DROP CONSTRAINT {column.Key};\nEND\n");

                        columnEntry += $" FOREIGN KEY REFERENCES {tblName}({columnName})";
                    }

                    if (!column.Value.nullable)
                        columnEntry += " NOT NULL";

                    if (column.Value.unique)
                        columnEntry += " UNIQUE";

                    if (column.Value.primaryKey)
                    {
                        columnEntry += " PRIMARY KEY";
                        hasPrimaryKey = true;

                       // if (dropExisting)
                       // {
                       //     droppablePrimaryKeys.Add($"IF OBJECT_ID(N'{obj.Value._tableAttribute?._sqlTableName}', N'U') IS NOT NULL\nBEGIN\nALTER TABLE {obj.Value._tableAttribute?._sqlTableName} DROP CONSTRAINT {column.Key};\nEND\n");
                        //}
                    }


                    if (column.Value.defaultValue != default(object))
                    {
                        columnEntry += " DEFAULT ";

                        var val = column.Value.defaultValue == null ? "NULL" : column.Value.defaultValue.ToString();
                        if (column.Value.type.ToLower().Contains("char(") ||
                            column.Value.type.ToLower().Contains("binary(") ||
                            column.Value.type.ToLower().Contains("blob") ||
                            column.Value.type.ToLower().Contains("text") ||
                            column.Value.type.ToLower().Contains("UniqueIdentifier"))
                        {
                            columnEntry += $"'{StringHelpers.Escape(val)}'";
                        }
                        else
                        {
                            if (column.Value.defaultValue is bool)
                                columnEntry += (bool)column.Value.defaultValue == true ? "1" : "0";
                            else
                                columnEntry += val;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(column.Value.index))
                    {
                        indexCreationEntries.Add($"CREATE INDEX idx_{column.Value.index} ON {obj.Value._tableAttribute?._sqlTableName}({column.Key});");

                    }

                    columnEntry += ",\n";
                    columns += columnEntry;
                }

                columns = columns.Substring(0, columns.Length - 2);


                


                _stringBuilder.AppendLine($"CREATE TABLE {obj.Value._tableAttribute?._sqlTableName} (\n{columns}\n);");
                _stringBuilder.AppendLine("GO");

                foreach (var indexEntry in indexCreationEntries)
                {
                    _stringBuilder.AppendLine(indexEntry);
                    _stringBuilder.AppendLine("GO");
                }





            }


        }

    }
}
