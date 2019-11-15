namespace Essence.Framework.System
{
    public interface ILazy<out T>
    {
        T Value { get; }
        bool IsValueCreated { get; }
    }
}