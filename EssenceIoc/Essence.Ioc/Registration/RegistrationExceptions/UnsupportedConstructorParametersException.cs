using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class UnsupportedConstructorParametersException : RegistrationException
    {
        public UnsupportedConstructorParametersException(Type classType)
            : base(
                $"There is no supported constructor in class {classType}. " +
                "Constructor must not have ref, out or optional parameters.")
        {
        }
    }
}