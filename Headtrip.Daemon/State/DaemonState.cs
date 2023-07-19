using Headtrip.Daemon.Models;
using Headtrip.Daemon.State.Abstract;
using Headtrip.GameServerContext;
using Headtrip.Models.Daemon;
using Headtrip.Models.Instance;
using Headtrip.Repositories.Abstract;
using Headtrip.Utilities;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.State
{
    public class DaemonState : IDaemonState
    {
        private readonly ILogging<HeadtripGameServerContext> _logging;

        private readonly IDaemonRepository _repository;
        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;
        
        private Headtrip.Models.Daemon.Daemon? _daemonObject;

        public DaemonState(
            ILogging<HeadtripGameServerContext> logging,
            IDaemonRepository repository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _logging = logging;
            _repository = repository;
            _gsUnitOfWork = gsUnitOfWork;
        }

        private string DaemonNickname = ConfigurationManager.AppSettings["DaemonNickname"] ?? string.Empty;

        public Dictionary<Guid, Channel> ChannelsByChannelId { get; private set; } = new Dictionary<Guid, Channel>();
        public Dictionary<Guid, UnrealServerInstance> ServersByChannelId { get; private set; } = new Dictionary<Guid, UnrealServerInstance>();












        public bool IsReady() => _daemonObject != null;
        public bool? IsSuperDaemon() => IsReady() ? _daemonObject.IsSuperDaemon : null;
        public string? GetDaemonNickname() => IsReady() ? _daemonObject.Nickname : null;
        public Guid? GetDaemonId() => IsReady() ? _daemonObject.DaemonId : null;


        public async Task<bool> Initialize()
        {
            if (IsReady())
                return true;

            try
            {
                if (string.IsNullOrWhiteSpace(DaemonNickname))
                    throw new Exception($"Daemon running on {System.Environment.MachineName} has no nickname specified in its configuration file.");


                _gsUnitOfWork.BeginTransaction();
                _daemonObject = await _repository.GetOrCreateDaemonByNickname(DaemonNickname);
                _gsUnitOfWork.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                _logging.LogException(ex);
                _gsUnitOfWork.RollbackTransaction();

                return false;
            }
        }
    }
}
