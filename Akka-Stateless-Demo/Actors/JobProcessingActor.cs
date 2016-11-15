﻿using System;
using Akka_Stateless_Demo.Messages;
using Akka_Stateless_Demo.Msm;

namespace Akka_Stateless_Demo.Actors
{
    public enum JobProcessingState
    {
        Running,
        Stopped
    }

    public class JobProcessingActor : StatelessHsmActor<JobProcessingState>
    {
        public JobProcessingActor()
            : base(JobProcessingState.Stopped)
        {
        }

        protected override void ConfigureStateMachine(MessageBasedStateMachine<JobProcessingState> sm)
        {
            sm.Configure(JobProcessingState.Stopped)
              .OnReceive<StartRequest>(request =>
              {
                  Console.WriteLine("Stopped : Starting Job");
                  return JobProcessingState.Running;
              })
              .OnReceive<StopRequest>(request =>
              {
                  Console.WriteLine("Running : Ignored Stop Request. Already Stopped!");
              })
                ;
            sm.Configure(JobProcessingState.Running)
              .OnReceive<StopRequest>(request =>
              {
                  Console.WriteLine("Running : Stopping Job");
                  return JobProcessingState.Stopped;
              })
              .OnReceive<StartRequest>(request =>
              {
                  Console.WriteLine("Running : Ignore Start Request. Already Running!");
              });
        }
    }
}