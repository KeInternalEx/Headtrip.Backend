using Headtrip.GameServerContext;
using Headtrip.Objects.Abstract.Results;
using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.UeMessages;
using Headtrip.UnrealService.Objects;
using Headtrip.UnrealService.State;
using Headtrip.UnrealService.Tasks.Abstract;
using Headtrip.UnrealService.Tasks.Interface;
using Headtrip.UnrealService.UnrealEngine.Management.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.Tasks
{
    public sealed class RequestAssignmentTask : AServiceTask, IRequestTransformationTask
    {

        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IContext<HeadtripGameServerContext> _Context;

        private readonly IUnrealServiceRepository _UnrealServiceRepository;
        private readonly IUnrealStrRepository _UeStrRepository;
        private readonly IUnrealLatencyRecordRepository _UeLatencyRecordRepository;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IZoneRepository _ZoneRepository;
        private readonly IUnrealServerFactory _UnrealServerFactory;

        public RequestAssignmentTask(
            ILogging<HeadtripGameServerContext> Logging,
            IContext<HeadtripGameServerContext> Context,
            IUnrealServiceRepository UnrealServiceRepository,
            IUnrealStrRepository UeStrRepository,
            IUnrealLatencyRecordRepository UeLatencyRecordRepository,
            IChannelRepository ChannelRepository,
            IZoneRepository ZoneRepository,
            IUnrealServerFactory ServerFactory) :
        base(
            UnrealServiceState.CancellationTokenSource.Value.Token,
            UnrealServiceConfiguration.RequestAssignmentTaskInterval)
        {
            _Logging = Logging;
            _Context = Context;
            _UnrealServiceRepository = UnrealServiceRepository;
            _UeStrRepository = UeStrRepository;
            _UeLatencyRecordRepository = UeLatencyRecordRepository;
            _ChannelRepository = ChannelRepository;
            _ZoneRepository = ZoneRepository;
            _UnrealServerFactory = ServerFactory;
        }

    
        private async Task<Stack<MUnrealServerTransferRequest>?> GetRequests()
        {
            try
            {
                var strs = await _UeStrRepository.ReadWithState(EUeServerTransferRequestState.PendingAssignment);
                return new Stack<MUnrealServerTransferRequest>(strs.Where((str) => str.CurrentUnrealServiceId == UnrealServiceState.ServiceId));
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
                return null;
            }
        }
       

        private async Task FailRequest(MUnrealServerTransferRequest request)
        {
            using (var transaction = _Context.BeginTransaction())
            {
                try
                {
                    request.State = EUeServerTransferRequestState.FailedAssignment;

                    await _UeStrRepository.Update(request);

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    _Logging.LogException(ex);
                    _Logging.LogWarning("Could not update server transfer request with failed state.");
                }
            }
        }

        private async Task CompleteRequest(MUnrealServerTransferRequest request)
        {
            using (var transaction = _Context.BeginTransaction())
            {
                try
                {
                    request.State = EUeServerTransferRequestState.Completed;

                    await _UeStrRepository.Update(request);

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    _Logging.LogException(ex);
                    _Logging.LogWarning("Could not update server transfer request with completed state.");
                }
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
                            var channel = await _ChannelRepository.Read(currentRequest.TargetChannelId!.Value);
                            var server = _UnrealServerFactory.GetByChannelId(channel.ChannelId);
                            if (server == null)
                            {
                                await FailRequest(currentRequest);
                                continue;   
                            }

                            await server.SendMessage(new MsgExecutePlayerTransfer(channel.ConnectionString, currentRequest.AccountId));
                            await CompleteRequest(currentRequest);
                        }
                    }
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
