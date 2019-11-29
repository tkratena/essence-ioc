using System;

namespace Essence.Ioc.ExtendableRegistration
{
    public abstract class Registerer
    {
        protected abstract void AddRegistration(IRegistration registration);

        public static void AddRegistration(Registerer registerer, IRegistration registration)
        {
            if (registerer == null)
            {
                throw new ArgumentNullException(nameof(registerer));
            }
            
            registerer.AddRegistration(registration);
        }
    }
}