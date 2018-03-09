using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class NonConcreteClassException : RegistrationException
    {
        public NonConcreteClassException(Type implementationType)
            : base($"{implementationType} is not a concrete class.")
        {
        }
    }
}