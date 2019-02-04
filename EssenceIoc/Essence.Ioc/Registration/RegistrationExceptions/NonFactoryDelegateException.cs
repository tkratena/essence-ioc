using Essence.Framework.System;

namespace Essence.Ioc.Registration.RegistrationExceptions
{
    internal class NonFactoryDelegateException : RegistrationException
    {
        public NonFactoryDelegateException(IDelegateInfo delegateInfo)
            : base($"Delegate {delegateInfo} is not supported. Delegates must have a return type and no parameters.")
        {
        }
    }
}