﻿using Headtrip.UeService.UnrealEngine;
using Headtrip.UeService.UnrealEngine.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeMessages.Inbound
{
    public sealed class HMsgPlayerJoin : AUnrealMessageHandler<MsgPlayerJoin>, IUnrealMessageHandler
    {

        public HMsgPlayerJoin() : base(MsgPlayerJoin.MsgType) { }
        protected async override Task HandleMessage(MsgPlayerJoin Message)
        {
            // TODO: NEED TO SEND THIGNG

        }
    }
}