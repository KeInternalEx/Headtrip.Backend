using Headtrip.UeService.State;
using Headtrip.UeService.Tasks.Abstract;
using Headtrip.UeService.Tasks.Interface;
using Headtrip.UeService.UnrealEngine.Management.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Tasks
{
    public sealed class MessagePollTask : AServiceTask, IMessagePollTask
    {

        private IUnrealMessageBusFactory _UnrealMessageBusFactory;
        
        public MessagePollTask(
            IUnrealMessageBusFactory UnrealMessageBusFactory) :
        base(
            UeServiceState.CancellationTokenSource.Value.Token,
            UeServiceConfiguration.RequestAssignmentTaskInterval)
        {
            _UnrealMessageBusFactory = UnrealMessageBusFactory;
        }


        protected async override Task Execute()
        {
            while (!_Token.IsCancellationRequested)
            {
                var pollers = _UnrealMessageBusFactory.GetPollers();
                if (pollers.Count() == 0)
                {
                    await Task.Delay(250); // Save cpu cycles if we have no poll methods to run
                    continue;
                }

                await Task.WhenAll(pollers.Select((p) => p.Poll(_Token)));
            }
        }
    }
}
