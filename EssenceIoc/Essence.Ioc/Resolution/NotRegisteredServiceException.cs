using System;

namespace Essence.Ioc.Resolution
{
    internal class NotRegisteredServiceException : Exception
    {
        public NotRegisteredServiceException(Type notRegisteredServiceType)
            : base(notRegisteredServiceType + " is not a registered service.")
        {
        }
    }
}