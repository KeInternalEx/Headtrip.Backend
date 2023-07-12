using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities.Abstract
{
    public interface ILogging<T>
    {

        void LogException(
            Exception exception,
            string Message = "", 
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0);
        
        void LogWarning(
            string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0);

        void LogInfo(
            string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0);



    }
}
