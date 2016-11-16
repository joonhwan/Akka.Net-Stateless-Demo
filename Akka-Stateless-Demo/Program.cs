using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka_Stateless_Demo.Actors;
using Akka_Stateless_Demo.Messages;

namespace Akka_Stateless_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("AkkaStatelessDemo");
            var actor = actorSystem.ActorOf(Props.Create(() => new JobProcessingActor()));
            
            Console.WriteLine("actor system is running....Ctrl-C to exit.");

            actor.Tell(new StartRequest("System1"));
            actor.Tell(new RunningStateProbeRequest());
            actor.Tell(new StartRequest("System2"));
            actor.Tell(new StopRequest("System3"));
            actor.Tell(new RunningStateProbeRequest());
            actor.Tell(new StopRequest("System4"));
            actor.Tell(new UnknownRequest());
            
            Console.CancelKeyPress += (sender, eventArgs) => {
                Console.WriteLine("Terminating...");
                actorSystem.Terminate().Wait();
            };
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
