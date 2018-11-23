using System;

namespace Essence.Ioc.FluentRegistration
{
    public interface IRegisterer
    {
        IService<TService> RegisterService<TService>() where TService : class;
        IGenericServices GenericallyRegisterService(Type genericServiceTypeDefinition);
    }
}