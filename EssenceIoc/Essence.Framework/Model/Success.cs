namespace Essence.Framework.Model
{
    public struct Success<TValue>
    {
        public TValue Value { get; }

        internal Success(TValue value)
        {
            Value = value;
        }
    }
}