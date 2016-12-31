namespace WinTail.Messages.Error
{
    public sealed class NullInputError : InputError
    {
        public NullInputError(string reason) : base(reason)
        {
        }
    }
}