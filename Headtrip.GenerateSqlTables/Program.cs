using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities;

namespace Headtrip.GenerateSqlTables
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sqlScriptWriter = new SqlCreationScriptWriter<DatabaseObject>("GS", true);
            var sqlScriptWriterLs = new SqlCreationScriptWriter<DatabaseObject>("LS", true);




            File.WriteAllText("GENGSDB.sql", sqlScriptWriter._stringBuilder.ToString());
            File.WriteAllText("GENLSDB.sql", sqlScriptWriterLs._stringBuilder.ToString());
        }
    }
}