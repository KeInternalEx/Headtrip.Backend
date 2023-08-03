using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Utilities.Interface;

namespace Headtrip.Utilities
{
    public class Logging<T> : ILogging<T>
    {
        private static readonly NLog.Logger _Logger = NLog.LogManager.GetLogger(typeof(T).Name);
        private static readonly string _LogContext =
            System.Configuration.ConfigurationManager.AppSettings["LogContext"] ?? "!!! NEED AppSettings.LogContext !!!";

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
            [CallerLineNumber] int LineNumber = 0)
        {
            _Logger.Error(exception, $"{_LogContext}: {Message} // {FilePath}::{MemberName} on line {LineNumber}");
        }

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
            [CallerLineNumber] int LineNumber = 0)
        {
            _Logger.Info($"{_LogContext}: {Message} // {FilePath}::{MemberName} on line {LineNumber}");
        }

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
            [CallerLineNumber] int LineNumber = 0)
        {
            _Logger.Warn($"{_LogContext}: {Message} // {FilePath}::{MemberName} on line {LineNumber}");
        }
    }
}
