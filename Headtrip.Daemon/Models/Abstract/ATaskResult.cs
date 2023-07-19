﻿using Headtrip.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Daemon.Models.Abstract
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

        public new ATaskResult BuildForException(Exception exception)
        {
            Exception = exception;
            Status = exception.Message;
            IsSuccessful = false;

            CalculateTimeSpent();

            return this;
        }
    }
}
