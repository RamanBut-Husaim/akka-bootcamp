namespace WinTail.Actors.TailCoordinator.Messages
{
    /// <summary>
    /// Stop tailing the file at user-specified path.
    /// </summary>
    public sealed class StopTail
    {
        public StopTail(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; private set; }
    }
}