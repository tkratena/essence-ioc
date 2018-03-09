using System;

namespace Essence.Ioc.FluentRegistration
{
    public interface IRegistrator
    {
        IService<TService> RegisterService<TService>() where TService : class;
        IGenericServices GenericlyRegisterService(Type genericServiceTypeDefinition);
    }
}