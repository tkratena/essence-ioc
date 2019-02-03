using System;

namespace Essence.Ioc.ExtendableRegistration
{
    public abstract class Registerer
    {
        protected abstract void AddRegistration(IRegistration registration);

        public interface IRegistration
        {
            void Register(IRegisterer registerer);
        }

        public sealed class Registrations
        {
            private readonly Registerer _registerer;

            public Registrations(Registerer registerer)
            {
                _registerer = registerer ?? throw new ArgumentNullException(nameof(registerer));
            }
        
            public void Add(IRegistration registration)
            {
                _registerer.AddRegistration(registration);
            }
        }
    }
}