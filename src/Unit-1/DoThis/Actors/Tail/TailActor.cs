using System.IO;
using System.Text;
using Akka.Actor;
using WinTail.Actors.Tail.Messages;

namespace WinTail.Actors.Tail
{
    public sealed class TailActor : UntypedActor
    {
        private readonly string filePath;
        private readonly IActorRef reporterActor;
        private readonly FileObserver observer;
        private readonly Stream fileStream;
        private readonly StreamReader fileStreamReader;

        public TailActor(IActorRef reporterActor, string filePath)
        {
            this.reporterActor = reporterActor;
            this.filePath = filePath;

            // start watching file for changes
            observer = new FileObserver(Self, Path.GetFullPath(this.filePath));
            observer.Start();

            // open the file stream with shared read/write permissions
            // (so file can be written to while open)
            fileStream = new FileStream(Path.GetFullPath(this.filePath),
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStreamReader = new StreamReader(fileStream, Encoding.UTF8);

            // read the initial contents of the file and send it to console as first msg
            var text = fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(this.filePath, text));
        }

        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                // move file cursor forward
                // pull results from cursor to end of file and write to output
                // (this is assuming a log file type format that is append-only)
                var text = fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    reporterActor.Tell(text);
                }

            }
            else if (message is FileError)
            {
                var fe = message as FileError;
                reporterActor.Tell($"Tail error: {fe.Reason}");
            }
            else if (message is InitialRead)
            {
                var ir = message as InitialRead;
                reporterActor.Tell(ir.Text);
            }
        }
    }
}