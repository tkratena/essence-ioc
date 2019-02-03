namespace Essence.Ioc.ExtendableRegistration
{
    public interface IRegistration
    {
        void Register(IBasicRegisterer registerer);
    }
}