using Headtrip.Daemon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Tasks.Abstract
{
    public interface IDaemonTask
    {
        Task<DaemonTaskResult> Execute();
    }
}
