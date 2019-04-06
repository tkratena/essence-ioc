namespace Essence.Framework.Model
{
    public struct Failure<TError>
    {
        public TError Error { get; }

        internal Failure(TError error)
        {
            Error = error;
        }
    }
}