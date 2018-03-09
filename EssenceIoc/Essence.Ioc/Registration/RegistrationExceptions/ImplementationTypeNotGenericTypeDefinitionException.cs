using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class ImplementationTypeNotGenericTypeDefinitionException : RegistrationException
    {
        public ImplementationTypeNotGenericTypeDefinitionException(Type implementationType)
            : base($"Implementation type {implementationType} is not a generic type definition.")
        {
        }
    }
}