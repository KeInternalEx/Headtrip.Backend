using Headtrip.GameServerContext;
using Headtrip.Objects.UnrealService;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.UnrealService.State;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers.Abstract;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers.Interface;
using Headtrip.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    public sealed class HMsgRequestPlayerTransfer : AUnrealMessageHandler<MsgRequestPlayerTransfer>, IUnrealMessageHandler
    {
        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IContext<HeadtripGameServerContext> _Context;
        private readonly IUnrealStrRepository _UnrealStrRepository;
        private readonly IAccountRepository _AccountRepository;
        private readonly ICharacterRepository _CharacterRepository;

        public HMsgRequestPlayerTransfer(
            ILogging<HeadtripGameServerContext> Logging,
            IContext<HeadtripGameServerContext> Context,
            IUnrealStrRepository UnrealStrRepository,
            IAccountRepository AccountRepository,
            ICharacterRepository CharacterRepository) :
            base(MsgRequestPlayerTransfer.MsgType)
        {
            _Logging = Logging;
            _Context = Context;
            _UnrealStrRepository = UnrealStrRepository;
            _AccountRepository = AccountRepository;
            _CharacterRepository = CharacterRepository;
        }

        protected async override Task HandleMessage(MsgRequestPlayerTransfer Message, CancellationToken Token)
        {
            try
            {
                using (var transaction = _Context.BeginTransaction())
                {
                    var account = await _AccountRepository.Read(Message.AccountId);
                    var character = await _CharacterRepository.Read(Message.CharacterId);

                    await _UnrealStrRepository.Create(new MUnrealServerTransferRequest
                    {
                        AccountId = account.AccountId,
                        CharacterId = character.CharacterId,
                        CharacterLevel = character.Level,
                        CurrentChannelId = _ServerInstance.ChannelId!.Value,
                        CurrentUnrealServiceId = UnrealServiceState.ServiceId,
                        PartyId = account.CurrentPartyId,
                        State = EUeServerTransferRequestState.PendingTransform,
                        ZoneName = Message.ZoneName,
                    });

                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex);
            }
        }
    }
}
