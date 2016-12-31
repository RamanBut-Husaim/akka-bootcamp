namespace WinTail.Messages.Success
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