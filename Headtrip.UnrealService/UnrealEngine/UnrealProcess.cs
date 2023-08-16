using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.UnrealEngine
{
    public sealed class UnrealProcess : IDisposable
    {
        private Process _Process;

        public void Dispose()
        {
            if (_Process != null)
            {
                _Process.Close();
                _Process.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public UnrealProcess(string ServerPath, string CommandLine)
            => _Process = Process.Start(ServerPath, CommandLine);


    }
}
