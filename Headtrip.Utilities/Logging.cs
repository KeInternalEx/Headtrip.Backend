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


        /// <summary>
        /// Log the given exception, automatically fills file path, member name, and line number.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="Message"></param>
        /// <param name="FilePath"></param>
        /// <param name="MemberName"></param>
        /// <param name="LineNumber"></param>
        public void LogException(
            Exception exception,
            string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0) =>
            _logger.Error(exception, $"{Message} // {FilePath}::{MemberName} on line {LineNumber}");

        /// <summary>
        /// Log information level, automatically fills file path, member name, and line number.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="FilePath"></param>
        /// <param name="MemberName"></param>
        /// <param name="LineNumber"></param>
        public void LogInfo(
            string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0) =>
            _logger.Info($"{Message} // {FilePath}::{MemberName} on line {LineNumber}");

        /// <summary>
        /// Log warning level, automatically fills file path, member name, and line number.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="FilePath"></param>
        /// <param name="MemberName"></param>
        /// <param name="LineNumber"></param>
        public void LogWarning(string Message,
            [CallerFilePath] string FilePath = "",
            [CallerMemberName] string MemberName = "",
            [CallerLineNumber] int LineNumber = 0) =>
            _logger.Warn($"{Message} // {FilePath}::{MemberName} on line {LineNumber}");
    }
}
