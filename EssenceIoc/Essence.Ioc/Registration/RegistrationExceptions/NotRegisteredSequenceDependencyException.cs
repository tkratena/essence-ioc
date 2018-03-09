using System;
using System.Linq;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class NotRegisteredSequenceDependencyException : RegistrationException
    {
        public NotRegisteredSequenceDependencyException(Type sequenceType)
            : base(
                $"{sequenceType} is not a registered service. " +
                $"If you would like to get all implementations of service {GetItemType(sequenceType)}, " +
                $"register an implementation of {sequenceType} explicitly.")
        {
        }

        private static Type GetItemType(Type sequenceType)
        {
            return sequenceType.GenericTypeArguments.Single();
        }
    }
}