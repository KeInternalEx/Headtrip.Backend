using Headtrip.Utilities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities
{
    public class Logging : ILogging
    {
        private readonly IContext _Context;


        public Logging(IContext context)
        {
            _Context = context;
        }



    }
}
