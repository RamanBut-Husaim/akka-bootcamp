namespace WinTail.Messages.Error
{
    public sealed class ValidationError : InputError
    {
        public ValidationError(string reason) : base(reason)
        {
        }
    }
}