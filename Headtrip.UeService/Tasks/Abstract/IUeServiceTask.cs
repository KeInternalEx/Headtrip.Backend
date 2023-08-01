using Headtrip.UeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Tasks.Abstract
{
    public interface IUeServiceTask
    {
        Task<UeServiceTaskResult> Execute();
    }
}
