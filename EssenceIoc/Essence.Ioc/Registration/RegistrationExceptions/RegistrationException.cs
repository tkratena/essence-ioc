using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal abstract class RegistrationException : Exception
    {
        protected RegistrationException(string message) : base(message)
        {
        }
        
        protected RegistrationException(string message, RegistrationException innerException)
            : base(message, innerException)
        {
        }
    }
}