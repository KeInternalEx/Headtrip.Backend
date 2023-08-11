using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.UeService;
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.UeService.Models;
using Headtrip.UeService.Models.Abstract.Results;
using Headtrip.UeService.Objects;
using Headtrip.UeService.State;
using Headtrip.UeService.Tasks.Abstract;
using Headtrip.UeService.Tasks.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Tasks
{
    public sealed class RequestAssignmentTask : AServiceTask, IRequestTransformationTask
    {

        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IUnitOfWork<HeadtripGameServerContext> _GsUnitOfWork;

        private readonly IUeServiceRepository _UeServiceRepository;
        private readonly IUeStrRepository _UeStrRepository;
        private readonly IUeLatencyRecordRepository _UeLatencyRecordRepository;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IZoneRepository _ZoneRepository;

        public RequestAssignmentTask(
            ILogging<HeadtripGameServerContext> Logging,
            IUnitOfWork<HeadtripGameServerContext> GsUnitOfWork,
            IUeServiceRepository UeServiceRepository,
            IUeStrRepository UeStrRepository,
            IUeLatencyRecordRepository UeLatencyRecordRepository,
            IChannelRepository ChannelRepository,
            IZoneRepository ZoneRepository) :
        base(
            UeServiceState.CancellationTokenSource.Value.Token,
            UeServiceConfiguration.RequestAssignmentTaskInterval)
        {
            _Logging = Logging;
            _GsUnitOfWork = GsUnitOfWork;
            _UeServiceRepository = UeServiceRepository;
            _UeStrRepository = UeStrRepository;
            _UeLatencyRecordRepository = UeLatencyRecordRepository;
            _ChannelRepository = ChannelRepository;
            _ZoneRepository = ZoneRepository;
        }

    
        private async Task<Stack<MUeServerTransferRequest>?> GetRequests()
        {
            try
            {
                var strs = await _UeStrRepository.ReadWithState(EUeServerTransferRequestState.PendingAssignment);
                return new Stack<MUeServerTransferRequest>(strs.Where((str) => str.CurrentUeServiceId == UeServiceState.ServiceId));
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                return null;
            }
        }
       

        protected async override Task Execute()
        {
            while (!_Token.IsCancellationRequested)
            {
                try
                {
                    var requests = await GetRequests();
                    if (requests != null)
                    {
                        while (requests.Count > 0)
                        {
                            var currentRequest = requests.Pop();
                            

                            if (!UeServiceState.ActiveServersByChannelId.TryGetValue(
                                currentRequest.CurrentChannelId,
                                out var server))
                            {
                                _Logging.LogWarning($"{UeServiceState.ServiceId} doesn't own {currentRequest.CurrentChannelId}");
                                continue;
                            }

                            var channel = await _ChannelRepository.Read(currentRequest.TargetChannelId!.Value);


                            server.Instance.
                        }
                    }
                    // DONE: get server transfer requests that have the pending assignment state AND have a CurrentUeServerid that we own
                    // DONE: lookup channel that the request wants to go to
                    // TODO: tell our server to connect that accountid to the requested server

                    // TODO: our server will notify us that a player left, which will decrement the number of players on our channel object
                    // TODO: the server being connected to will notify its service, which will increment its channel's number of players
                    // TODO: it will also tell the service to update the account's current channel id
                    // TODO: it will also tell the service to update the account's, character's current zone name



                }
                catch (Exception ex)
                {
                    _Logging.LogException(ex);
                }
                finally
                {
                    Thread.Sleep(_Interval);
                }
            }
        }


    }
}
