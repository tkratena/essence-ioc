namespace Essence.Framework.Model
{
    public struct Success<TValue>
    {
        public TValue Value { get; }

        public Success(TValue value)
        {
            Value = value;
        }
    }
}