# Akka.Net-Stateless-Demo

POC project for using Stateless with Akka.net

New ```StatelessHsmActor<T>``` actor class has been implemented where ```ReceiveActor``` and ```MessageBasedStateMachine<T>```
(that is wrapper of ```Statless.StateMachine```)

```MessageBasedStateMachine<T>`` in particular itself enable ```Stateless.StateMachine``` to handle specific *typed* message object as trigger(Originally, *Stateless* uses Enum'd value)


Sample code follows. 

```
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
    ```
    
