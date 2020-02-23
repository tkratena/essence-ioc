using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class DisposableServiceException : RegistrationException
    {
        public DisposableServiceException(Type serviceType)
            : base(
                $"Service {serviceType} must not be disposable. " +
                "Make only the implementation disposable and let the container take care of its life cycle. " +
                "Or create a factory for the disposable, inject that as a service instead " +
                "and take care of the disposal yourself when you use the factory.")
        {
        }
    }
}