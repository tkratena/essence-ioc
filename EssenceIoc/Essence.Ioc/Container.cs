using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.Registration;
using Essence.Ioc.Resolution;

namespace Essence.Ioc
{
    public sealed class Container : IContainer, IDisposable
    {
        private readonly Resolver _resolver;
        private readonly InstanceTracker _tracker = new InstanceTracker();
        private bool _isDisposed;

        public Container(Action<ExtendableRegistration.Registerer> serviceRegistration)
        {
            var factories = new Factories();

            _resolver = new Resolver(factories, _tracker);
            
            var registerer = new Registration.Registerer(factories, _resolver, _tracker);
            var extendableRegisterer = new ExtendableRegisterer(registerer);
            
            serviceRegistration.Invoke(extendableRegisterer);
            extendableRegisterer.ExecuteRegistrations();
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(Container));
            }
            
            return _resolver.Resolve<TService>();
        }
        
        public void Dispose()
        {
            _tracker.DisposeTrackedDisposables();
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