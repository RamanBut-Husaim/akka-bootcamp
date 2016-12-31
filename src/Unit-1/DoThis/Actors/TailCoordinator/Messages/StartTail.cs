using Akka.Actor;

namespace WinTail.Actors.TailCoordinator.Messages
{
    /// <summary>
    /// Start tailing the file at user-specified path.
    /// </summary>
    public sealed class StartTail
    {
        public StartTail(string filePath, IActorRef reporterActor)
        {
            FilePath = filePath;
            ReporterActor = reporterActor;
        }

        public string FilePath { get; private set; }

        public IActorRef ReporterActor { get; private set; }
    }
}