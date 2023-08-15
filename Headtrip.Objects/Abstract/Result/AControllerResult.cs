using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Abstract.Results
{
    public abstract class AControllerResult
    {
        public required string Status { get; set; }
    }
}
