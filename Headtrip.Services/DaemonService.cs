using Headtrip.GameServerContext;
using Headtrip.Models.Daemon;
using Headtrip.Repositories.Abstract;
using Headtrip.Services.Abstract;
using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Services
{
    public class DaemonService : IDaemonService
    {
        private readonly IDaemonRepository _daemonRepository;
        private readonly IUnitOfWork<HeadtripGameServerContext> _gsUnitOfWork;

        public DaemonService(IDaemonRepository daemonRepository, IUnitOfWork<HeadtripGameServerContext> gsUnitOfWork)
        {
            _daemonRepository = daemonRepository;
            _gsUnitOfWork = gsUnitOfWork;
        }





    }
}
