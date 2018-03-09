using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Ioc.Registration;

namespace Essence.Ioc.FluentRegistration
{
    internal class FluentRegistrator : IRegistrator
    {
        private readonly ICollection<IRegistration> _registrations = new List<IRegistration>();

        internal void ExecuteRegistration(Registrator registrator)
        {
            foreach (var registration in _registrations)
            {
                registration.Register(registrator);
            }
        }
        
        [Pure]
        public IService<TService> RegisterService<TService>() where TService : class
        {
            return new Service<TService>(_registrations);
        }

        [Pure]
        public IGenericServices GenericlyRegisterService(Type genericServiceTypeDefinition)
        {
            return new GenericService(_registrations, genericServiceTypeDefinition);
        }
    }
}