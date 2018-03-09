using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class ImplementationTypeNotImplementingGenericService : RegistrationException
    {
        public ImplementationTypeNotImplementingGenericService(Type implementationType, Type serviceType)
            : base($"Implementation type {implementationType} does not implement the service type {serviceType}.")
        {
        }
    }
}