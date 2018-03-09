using System;
using System.Collections.Generic;
using Essence.Ioc.Registration;

namespace Essence.Ioc.FluentRegistration
{
    internal abstract class ServiceBase
    {
        private readonly IEnumerable<Type> _serviceTypes;
            
        protected ICollection<IRegistration> Registrations { get; }

        protected ServiceBase(ICollection<IRegistration> registrations, IEnumerable<Type> serviceTypes)
        {
            Registrations = registrations;
            _serviceTypes = serviceTypes;
        }

        protected ILifeScope AddImplementation<TServiceImplementation>()
            where TServiceImplementation : class
        {
            var registration = new Implementation<TServiceImplementation>(_serviceTypes);
            Registrations.Add(registration);
            return registration;
        }

        protected ILifeScope AddFactory<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class
        {
            var registration = new Factory<TServiceImplementation>(factory, _serviceTypes);
            Registrations.Add(registration);
            return registration;
        }

        protected ILifeScope AddFactory<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory) 
            where TServiceImplementation : class
        {
            var registration = new FactoryUsingContainer<TServiceImplementation>(factory, _serviceTypes);
            Registrations.Add(registration);
            return registration;
        }
            
        private class Implementation<TImplementation> : RegistrationBase
            where TImplementation : class
        {
            public Implementation(IEnumerable<Type> serviceTypes) : base(serviceTypes)
            {
            }

            protected override void RegisterTransient(Registrator registrator, IEnumerable<Type> servicesTypes)
            {
                registrator.RegisterTransient(typeof(TImplementation), servicesTypes);
            }

            protected override void RegisterSingleton(Registrator registrator, IEnumerable<Type> servicesTypes)
            {
                registrator.RegisterSingleton(typeof(TImplementation), servicesTypes);
            }
        }

        private class Factory<TImplementation> : RegistrationBase
            where TImplementation : class
        {
            private readonly Func<TImplementation> _factory;

            public Factory(Func<TImplementation> factory, IEnumerable<Type> serviceTypes) : base(serviceTypes)
            {
                _factory = factory;
            }

            protected override void RegisterTransient(Registrator registrator, IEnumerable<Type> servicesTypes)
            {
                registrator.RegisterFactoryTransient(_factory, servicesTypes);
            }

            protected override void RegisterSingleton(Registrator registrator, IEnumerable<Type> servicesTypes)
            {
                registrator.RegisterFactorySingleton(_factory, servicesTypes);
            }
        }
            
        private class FactoryUsingContainer<TImplementation> : RegistrationBase 
            where TImplementation : class
        {
            private readonly Func<IContainer, TImplementation> _factory;

            public FactoryUsingContainer(Func<IContainer, TImplementation> factory, IEnumerable<Type> serviceTypes)
                : base(serviceTypes)
            {
                _factory = factory;
            }

            protected override void RegisterTransient(Registrator registrator, IEnumerable<Type> servicesTypes)
            {
                registrator.RegisterFactoryTransient(_factory, servicesTypes);
            }

            protected override void RegisterSingleton(Registrator registrator, IEnumerable<Type> servicesTypes)
            {
                registrator.RegisterFactorySingleton(_factory, servicesTypes);
            }
        }

        private abstract class RegistrationBase : IRegistration, ILifeScope
        {
            private readonly IEnumerable<Type> _serviceTypes;
            private bool _asSingleton;

            protected RegistrationBase(IEnumerable<Type> serviceTypes)
            {
                _serviceTypes = serviceTypes;
            }
        
            public void AsSingleton()
            {
                _asSingleton = true;
            }

            public void Register(Registrator registrator)
            {
                if (_asSingleton)
                {
                    RegisterSingleton(registrator, _serviceTypes);
                }
                else
                {
                    RegisterTransient(registrator, _serviceTypes);
                }
            }

            protected abstract void RegisterTransient(Registrator registrator, IEnumerable<Type> servicesTypes);
            
            protected abstract void RegisterSingleton(Registrator registrator, IEnumerable<Type> servicesTypes);
        }
    }
}