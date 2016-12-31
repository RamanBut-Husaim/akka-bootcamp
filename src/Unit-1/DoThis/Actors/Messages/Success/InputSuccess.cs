namespace WinTail.Actors.Messages.Success
{
    public class InputSuccess
    {
        public InputSuccess(string reason)
        {
            this.Reason = reason;
        }

        public string Reason { get; }
    }
}