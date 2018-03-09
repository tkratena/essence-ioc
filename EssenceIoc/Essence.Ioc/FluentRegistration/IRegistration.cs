using Essence.Ioc.Registration;

namespace Essence.Ioc.FluentRegistration
{
    internal interface IRegistration
    {
        void Register(Registrator registrator);
    }
}