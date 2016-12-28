using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    public sealed class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";

        private readonly IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            if (consoleWriterActor == null)
                throw new ArgumentNullException(nameof(consoleWriterActor));

            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var read = Console.ReadLine();

            if (!string.IsNullOrEmpty(read) && string.Equals(read, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // shut down the system (acquire handle to system via
                // this actors context)
                Context.System.Terminate();
                return;
            }

            // send input to the console writer to process and print
            _consoleWriterActor.Tell(read);

            // continue reading messages from the console
            Self.Tell("continue");
        }

    }
}