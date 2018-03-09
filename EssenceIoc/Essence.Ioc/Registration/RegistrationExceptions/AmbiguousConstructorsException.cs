using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class AmbiguousConstructorsException : RegistrationException
    {
        public AmbiguousConstructorsException(Type classType)
            : base($"Ambiguous constructors of class {classType}.")
        {
        }
    }
}