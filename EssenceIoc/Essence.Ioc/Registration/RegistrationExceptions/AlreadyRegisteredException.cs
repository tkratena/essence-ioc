using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class AlreadyRegisteredException : RegistrationException
    {
        public AlreadyRegisteredException(Type serviceType)
            : base($"Service {serviceType} has been already registered.")
        {
        }
    }
}