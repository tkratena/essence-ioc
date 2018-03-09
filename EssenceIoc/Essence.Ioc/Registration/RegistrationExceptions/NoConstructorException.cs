using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class NoConstructorException : RegistrationException
    {
        public NoConstructorException(Type classType)
            : base($"No public constructor of class {classType}.")
        {
        }
    }
}