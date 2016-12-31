namespace WinTail.Messages.Error
{
    public class InputError
    {
        public InputError(string reason)
        {
            this.Reason = reason;
        }

        public string Reason { get; }
    }
}