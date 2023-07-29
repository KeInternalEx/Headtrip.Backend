using Headtrip.Objects.Abstract.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UeService.Models.Abstract
{
    public abstract class ATaskResult : AServiceCallResult
    {
        private readonly DateTime _creationTime;
        public TimeSpan TimeSpent { get; set; }



        public ATaskResult()
        {
            _creationTime = DateTime.Now;
        }


        public void CalculateTimeSpent() =>
            TimeSpent = DateTime.Now - _creationTime;

        public ATaskResult BuildForException(Exception exception)
        {
            Exception = exception;
            Status = exception.Message;
            IsSuccessful = false;

            CalculateTimeSpent();

            return this;
        }
    }
}
