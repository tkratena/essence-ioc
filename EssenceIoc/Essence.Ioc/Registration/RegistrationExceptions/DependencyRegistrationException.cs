using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class DependencyRegistrationException : RegistrationException
    {
        public DependencyRegistrationException(Type injectionTarget, RegistrationException innerException) 
            : base($"A dependency of {injectionTarget} is not supported.", innerException)
        {
        }
    }
}