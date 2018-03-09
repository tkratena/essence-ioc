using System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class NotRegisteredDependencyException : RegistrationException
    {
        public Type DependencyType { get; }

        public NotRegisteredDependencyException(Type dependencyType)
            : base($"{dependencyType} is not a registered service.")
        {
            DependencyType = dependencyType;
        }
    }
}