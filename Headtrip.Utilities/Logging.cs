using Dapper;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities
{
    public class Logging<T> : ILogging<T>
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetLogger(typeof(T).Name);



        public void LogException(
            Exception exception,
            string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0) =>
            _logger.Error(exception, $"{Message} // {FilePath}::{MemberName} on line {LineNumber}");

        public void LogInfo(
            string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0) =>
            _logger.Info($"{Message} // {FilePath}::{MemberName} on line {LineNumber}");

        public void LogWarning(string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0) =>
            _logger.Warn($"{Message} // {FilePath}::{MemberName} on line {LineNumber}");
    }
}
