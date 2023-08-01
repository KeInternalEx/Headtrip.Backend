using Headtrip.UeService.Models;
using Headtrip.UeService.State.Abstract;
using Headtrip.UeService.UnrealEngine;
using Headtrip.GameServerContext;
using Headtrip.Objects.UeService;
using Headtrip.Objects.Instance;
using Headtrip.Utilities;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Repositories.Repositories.Interface.GameServer;

namespace Headtrip.UeService.State
{
    public sealed class UeServiceState : IUeServiceState
    {
        private readonly ILogging<HeadtripGameServerContext> _logging;
        private readonly IUeServiceRepository _repository;
        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;
        
        private Headtrip.Objects.UeService.MUeService? _UeServiceObject;

        private string _UeServiceNickname = ConfigurationManager.AppSettings["UeServiceNickname"] ?? string.Empty;

        public Dictionary<Guid, Channel> ChannelsByChannelId { get; private set; } =
            new Dictionary<Guid, Channel>();

        public Dictionary<Guid, UnrealServerInstance> ServersByChannelId { get; private set; } =
            new Dictionary<Guid, UnrealServerInstance>();

        public bool IsReady() => _UeServiceObject != null;
        public bool? IsSuperUeService() => IsReady() ? _UeServiceObject.IsSuperUeService : null;
        public string? GetUeServiceNickname() => IsReady() ? _UeServiceObject.Nickname : null;
        public Guid? GetUeServiceId() => IsReady() ? _UeServiceObject.UeServiceId : null;

        public UeServiceState(
            ILogging<HeadtripGameServerContext> logging,
            IUeServiceRepository repository,
            IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _logging = logging;
            _repository = repository;
            _gsUnitOfWork = gsUnitOfWork;
        }

        public async Task<bool> Initialize()
        {
            if (IsReady())
                return true;

            try
            {
                if (string.IsNullOrWhiteSpace(_UeServiceNickname))
                    throw new Exception($"UeService running on {System.Environment.MachineName} has no nickname specified in its configuration file.");


                _gsUnitOfWork.BeginTransaction();
                _UeServiceObject = await _repository.GetOrCreateUeServiceByNickname(_UeServiceNickname);
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
