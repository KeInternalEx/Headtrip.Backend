﻿using Headtrip.UeService.UnrealEngine.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    public sealed class HMsgRequestPlayerTransfer : AUnrealMessageHandler<MsgRequestPlayerTransfer>, IUnrealMessageHandler
    {

        public HMsgRequestPlayerTransfer() : base(MsgRequestPlayerTransfer.MsgType) { }
        protected async override Task HandleMessage(MsgRequestPlayerTransfer Message)
        {
            // TODO: NEED TO SEND THIGNG

        }
    }
}