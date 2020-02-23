using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class InvalidRegistrationAfterContainerConstructedException : InvalidOperationException
    {
        public InvalidRegistrationAfterContainerConstructedException()
            : base("The container is immutable and registration is not possible when already constructed")
        {
        }
    }
}