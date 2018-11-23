using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Ioc.Registration;

namespace Essence.Ioc.FluentRegistration
{
    internal class FluentRegisterer : IRegisterer
    {
        private readonly ICollection<IRegistration> _registrations = new List<IRegistration>();

        internal void ExecuteRegistration(Registerer registerer)
        {
            foreach (var registration in _registrations)
            {
                registration.Register(registerer);
            }
        }
        
        [Pure]
        public IService<TService> RegisterService<TService>() where TService : class
        {
            return new Service<TService>(_registrations);
        }

        [Pure]
        public IGenericServices GenericallyRegisterService(Type genericServiceTypeDefinition)
        {
            return new GenericService(_registrations, genericServiceTypeDefinition);
        }
    }
}