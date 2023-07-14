using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon
{
    public static class DaemonState
    {

        public static string DAEMON_NICKNAME { get; private set; } = ConfigurationManager.AppSettings["DaemonNickname"] ?? string.Empty;
        public static Guid DaemonId { get; set; }
        public static bool IsSuperDaemon { get; set; }


        public static bool ValidateInitialDaemonState() =>
            !string.IsNullOrEmpty(DAEMON_NICKNAME);


        public static async Task<bool> InitializeDaemon()
        {



        }
    }
}
