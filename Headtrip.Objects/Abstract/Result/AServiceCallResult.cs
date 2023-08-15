using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Abstract.Results
{
    public abstract class AServiceCallResult
    {
        public bool IsSuccessful { get; set; } = false;
        public string Status { get; set; } = string.Empty;
        public Exception? Exception { get; set; } = null;


        public static T BuildForException<T>(Exception e) where T : AServiceCallResult, new()
        {
            return new T
            {
                Exception = e,
                Status = e.Message,
                IsSuccessful = false
            };
        }
    }
}
