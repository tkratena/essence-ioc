namespace Essence.Framework.Model
{
    public struct Failure<TError>
    {
        public TError Error { get; }

        public Failure(TError error)
        {
            Error = error;
        }
    }
    
    public struct Failure
    {
    }
}