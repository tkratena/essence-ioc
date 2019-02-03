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
            
            var basicRegisterer = new BasicRegisterer(factories, _resolver);
            var registerer = new Registerer(basicRegisterer);
            
            serviceRegistration.Invoke(registerer);
            registerer.ExecuteRegistrations();
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            return _resolver.Resolve<TService>();
        }
        
        private class Registerer : ExtendableRegistration.Registerer
        {
            private readonly Registrations _registrations;

            public Registerer(IBasicRegisterer basicRegisterer)
            {
                _registrations = new Registrations(basicRegisterer);
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
            private readonly IBasicRegisterer _basicRegisterer;
            private readonly ICollection<IRegistration> _registrations = new List<IRegistration>();

            public Registrations(IBasicRegisterer basicRegisterer)
            {
                _basicRegisterer = basicRegisterer;
            }

            public override void Add(IRegistration registration)
            {
                _registrations.Add(registration);
            }
                
            public void ExecuteRegistrations()
            {
                foreach (var registration in _registrations)
                {
                    registration.Register(_basicRegisterer);
                }
            }
        }
    }
}