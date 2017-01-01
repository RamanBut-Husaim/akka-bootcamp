using System;
using System.IO;
using Akka.Actor;
using WinTail.Actors.Messages.Error;
using WinTail.Actors.Messages.Neutral;
using WinTail.Actors.Messages.Success;
using WinTail.Actors.TailCoordinator.Messages;

namespace WinTail.Actors
{
    public sealed class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef consoleWriterActor;

        public FileValidatorActor(IActorRef consoleWriterActor)
        {
            if (consoleWriterActor == null)
                throw new ArgumentNullException(nameof(consoleWriterActor));

            this.consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // signal that the user needs to supply an input
                consoleWriterActor.Tell(new NullInputError("Input was blank. Please try again.\n"));

                // tell sender to continue doing its thing (whatever that may be,
                // this actor doesn't care)
                Sender.Tell(new ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(msg);
                if (valid)
                {
                    // signal successful input
                    consoleWriterActor.Tell(new InputSuccess($"Starting processing for {msg}"));

                    // start coordinator
                    Context
                        .ActorSelection("akka://MyActorSystem/user/tailCoordinatorActor")
                        .Tell(new StartTail(msg, consoleWriterActor));
                }
                else
                {
                    // signal that input was bad
                    consoleWriterActor.Tell(new ValidationError($"{msg} is not an existing URI on disk."));

                    // tell sender to continue doing its thing (whatever that
                    // may be, this actor doesn't care)
                    Sender.Tell(new ContinueProcessing());
                }
            }
        }

        /// <summary>
        /// Checks if file exists at path provided by user.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}