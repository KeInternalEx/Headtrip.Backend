using Headtrip.UnrealService.State;
using Headtrip.UnrealService.Tasks.Abstract;
using Headtrip.UnrealService.Tasks.Interface;
using Headtrip.UnrealService.UnrealEngine.Management.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.Tasks
{
    public sealed class MessagePollTask : AServiceTask, IMessagePollTask
    {

        private IUnrealMessageBusFactory _UnrealMessageBusFactory;
        
        public MessagePollTask(
            IUnrealMessageBusFactory UnrealMessageBusFactory) :
        base(
            UnrealServiceState.CancellationTokenSource.Value.Token,
            UnrealServiceConfiguration.RequestAssignmentTaskInterval)
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
