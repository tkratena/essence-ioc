using System;
using System.Diagnostics.Contracts;
using Essence.Ioc.ExtendableRegistration;

namespace Essence.Ioc.FluentRegistration
{
    public static class FluentRegistererExtensions
    {
        [Pure]
        public static IService<TService> RegisterService<TService>(this Registerer registerer) where TService : class
        {
            return new Service<TService>(new Registerer.Registrations(registerer));
        }

        [Pure]
        public static IGenericServices GenericallyRegisterService(
            this Registerer registerer, 
            Type genericServiceTypeDefinition)
        {
            return new GenericService(new Registerer.Registrations(registerer), genericServiceTypeDefinition);
        }
    }
}