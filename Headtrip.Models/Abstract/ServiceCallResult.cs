using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.Abstract
{
    public class ServiceCallResult
    {
        public required bool IsSuccessful { get; set; }
        public required string Status { get; set; }
        public Exception? Exception { get; set; }


        public static T BuildForException<T>(Exception e) where T: ServiceCallResult
        {
            return (T)new ServiceCallResult
            {
                Exception = e,
                Status = e.Message,
                IsSuccessful = false
            };
        }
    }
}
