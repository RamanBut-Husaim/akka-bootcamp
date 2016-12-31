namespace WinTail.Actors.Messages.Error
{
    public sealed class NullInputError : InputError
    {
        public NullInputError(string reason) : base(reason)
        {
        }
    }
}