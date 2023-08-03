using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.UeService.Tasks.Interface;
using Headtrip.UeService.Threading;

namespace Headtrip.UeService.Tasks.Abstract
{
    public abstract class AServiceTask : IServiceTask
    {
        protected readonly CancellationToken _Token;
        protected readonly int _Interval;

        private readonly Thread _Thread;
        private TsObject<bool> _IsRunning;


        public bool IsRunning { get { return _IsRunning.Value; } }

        protected AServiceTask(CancellationToken token, int interval)
        {
            _Token = token;
            _Interval = interval;
            _IsRunning = new TsObject<bool>(false);

            _Thread = new Thread(async () =>
            {
                _IsRunning.Value = true;

                await Execute();

                _IsRunning.Value = false;
            });

            _Thread.Start();
        }

        protected abstract Task Execute();


        public void WaitForCompletion() =>
            _Thread.Join();
    }
}
