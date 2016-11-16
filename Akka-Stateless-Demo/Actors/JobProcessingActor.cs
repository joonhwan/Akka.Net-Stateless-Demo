using System;
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
                  Console.WriteLine("Stopped : Starting Job : {0}", request);
                  return JobProcessingState.Running;
              })
              .OnReceive<StopRequest>(request =>
              {
                  Console.WriteLine("Running : Ignored Stop Request. Already Stopped! : {0}", request);
              })
                ;
            sm.Configure(JobProcessingState.Running)
              .OnReceive<StopRequest>(request =>
              {
                  Console.WriteLine("Running : Stopping Job : {0}", request);
                  return JobProcessingState.Stopped;
              })
              .OnReceive<StartRequest>(request =>
              {
                  Console.WriteLine("Running : Ignore Start Request. Already Running! : {0}", request);
              })
              .OnReceive<RunningStateProbeRequest>(request =>
              {
                  Console.WriteLine("Running : Pinged! : {0}", request);
              })
                ;
            sm.Transitioned += info =>
            {
                Console.WriteLine("--- Transitioned : {0}", info);
            };
            sm.Unhandled += (state, o) =>
            {
                Console.WriteLine("--- Unhandled message : State={0}, Message={1}", state, o);
            };
        }
    }
}