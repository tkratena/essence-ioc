using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.Registration;
using Essence.Ioc.Resolution;

namespace Essence.Ioc
{
    public sealed class Container : IDisposable
    {
        private readonly Resolver _resolver;
        private readonly LifeScope _singletonLifeScope = new LifeScope();
        private bool _isDisposed;

        public Container(Action<ExtendableRegistration.Registerer> serviceRegistration)
        {
            var factories = new Factories();

            _resolver = new Resolver(factories);
            
            var registerer = new Registration.Registerer(factories, _resolver, _singletonLifeScope);
            var extendableRegisterer = new ExtendableRegisterer(registerer);
            
            serviceRegistration.Invoke(extendableRegisterer);
            extendableRegisterer.ExecuteRegistrations();
        }

        [Pure]
        public IDisposable Resolve<TService>(out TService service) where TService : class
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(Container));
            }
            
            var transientLifeScope = new LifeScope();
            service = _resolver.Resolve<TService>(transientLifeScope);
            return transientLifeScope;
        }
        
        public void Dispose()
        {
            _singletonLifeScope.Dispose();
            _isDisposed = true;
        }

        private class ExtendableRegisterer : ExtendableRegistration.Registerer
        {
            private readonly IRegisterer _registerer;
            private readonly ICollection<IRegistration> _registrations = new List<IRegistration>();

            public ExtendableRegisterer(IRegisterer registerer)
            {
                _registerer = registerer;
            }

            protected override void AddRegistration(IRegistration registration)
            {
                lock (_registrations)
                {
                    _registrations.Add(registration);
                }
            }

            public void ExecuteRegistrations()
            {
                lock (_registrations)
                {
                    foreach (var registration in _registrations)
                    {
                        registration.Register(_registerer);
                    }
                }
            }
        }
    }
}