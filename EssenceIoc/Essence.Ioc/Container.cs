using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.Registration;
using Essence.Ioc.Resolution;

namespace Essence.Ioc
{
    public class Container : IContainer
    {
        private readonly IContainer _resolver;

        public Container(Action<ExtendableRegistration.Registerer> serviceRegistration)
        {
            var factories = new Factories();
            _resolver = new Resolver(factories);
            
            var registerer = new Registration.Registerer(factories, _resolver);
            var extendableRegisterer = new ExtendableRegisterer(registerer);
            
            serviceRegistration.Invoke(extendableRegisterer);
            extendableRegisterer.ExecuteRegistrations();
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            return _resolver.Resolve<TService>();
        }
        
        private class ExtendableRegisterer : ExtendableRegistration.Registerer
        {
            private readonly Registrations _registrations;

            public ExtendableRegisterer(IRegisterer registerer)
            {
                _registrations = new Registrations(registerer);
            }

            public void ExecuteRegistrations()
            {
                _registrations.ExecuteRegistrations();
            }

            [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")] 
            protected override ExtendableRegistration.Registrations Registrations => _registrations;
        }
        
        private class Registrations : ExtendableRegistration.Registrations
        {
            private readonly IRegisterer _registerer;
            private readonly ICollection<IRegistration> _registrations = new List<IRegistration>();

            public Registrations(IRegisterer registerer)
            {
                _registerer = registerer;
            }

            public override void Add(IRegistration registration)
            {
                _registrations.Add(registration);
            }
                
            public void ExecuteRegistrations()
            {
                foreach (var registration in _registrations)
                {
                    registration.Register(_registerer);
                }
            }
        }
    }
}