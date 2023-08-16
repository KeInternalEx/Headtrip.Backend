using Headtrip.GameServerContext;
using Headtrip.Repositories.Repositories.Interface;
using Headtrip.UnrealService.UnrealEngine;
using Headtrip.UnrealService.UnrealEngine.MessageHandlers;
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
    public sealed class HMsgPlayerJoin : AUnrealMessageHandler<MsgPlayerJoin>, IUnrealMessageHandler
    {
        private readonly ILogging<HeadtripGameServerContext> _Logging;
        private readonly IContext<HeadtripGameServerContext> _Context;
        private readonly IChannelRepository _ChannelRepository;
        private readonly IAccountRepository _AccountRepository;
        private readonly ICharacterRepository _CharacterRepository;


        public HMsgPlayerJoin(
            ILogging<HeadtripGameServerContext> Logging,
            IContext<HeadtripGameServerContext> Context,
            IChannelRepository ChannelRepository,
            IAccountRepository AccountRepository,
            ICharacterRepository CharacterRepository) :
        base(
            MsgPlayerJoin.MsgType)
        {
            _Logging = Logging;
            _Context = Context;
            _ChannelRepository = ChannelRepository;
            _AccountRepository = AccountRepository;
            _CharacterRepository = CharacterRepository;
        }

        protected async override Task HandleMessage(MsgPlayerJoin Message, CancellationToken Token)
        {
            try
            {
                using (var transaction = _Context.BeginTransaction())
                {
                    var account = await _AccountRepository.Read(Message.AccountId);
                    if (account == null)
                        throw new Exception($"No account w/ id {Message.AccountId}");
                    
                    var character = await _CharacterRepository.Read(Message.CharacterId);
                    if (character == null)
                        throw new Exception($"No character w/ id {Message.CharacterId}");


                    character.LLOCZoneName = _ServerInstance.ZoneName;
                    account.CurrentChannelId = _ServerInstance.ChannelId;

                    await _ChannelRepository.IncrementPlayerCount(_ServerInstance.ChannelId!.Value);
                    await _CharacterRepository.Update(character);
                    await _AccountRepository.Update(account);
                    
                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                _Logging.LogException(ex, $"Error while updating database for player join ({Message.AccountId}) on {_ServerInstance.ServerId}");
            }

        }
    }
}
