using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class DisposableClassException : RegistrationException
    {
        public DisposableClassException(Type serviceType)
            : base(
                $"Service {serviceType} must not be disposable. " +
                "Consider creating a factory for the disposable class and have that as a service instead. " +
                "That way, you can make use of the using pattern and take care of dispose properly.")
        {
        }
    }
}