using System;
using System.Collections.Generic;
using System.Linq;
using Essence.Framework.Linq;
using Essence.Ioc.Registration;

namespace Essence.Ioc.FluentRegistration
{
    internal class GenericService : IGenericServices
    {
        private readonly ICollection<IRegistration> _registrations;
        private readonly IEnumerable<Type> _genericServiceTypeDefinitions;

        public GenericService(ICollection<IRegistration> registrations, Type genericServiceTypeDefinition)
            : this(registrations, genericServiceTypeDefinition.UnfoldToEnumerable())
        {
        }
            
        private GenericService(
            ICollection<IRegistration> registrations,
            IEnumerable<Type> genericServiceTypeDefinitions)
        {
            _registrations = registrations;
            _genericServiceTypeDefinitions = genericServiceTypeDefinitions;
        }
            
        public void ImplementedBy(Type genericServiceImplementationTypeDefinition)
        {
            _registrations.Add(new GenericImplementation(
                genericServiceImplementationTypeDefinition,
                _genericServiceTypeDefinitions));
        }

        public IGenericServices AndService(Type genericServiceTypeDefinition)
        {
            return new GenericService(
                _registrations, 
                _genericServiceTypeDefinitions.Append(genericServiceTypeDefinition));
        }
            
        private class GenericImplementation : IRegistration
        {
            private readonly Type _implementationGenericTypeDefinition;
            private readonly IEnumerable<Type> _serviceGenericTypeDefinitions;

            public GenericImplementation(
                Type implementationGenericTypeDefinition, 
                IEnumerable<Type> serviceGenericTypeDefinitions)
            {
                _implementationGenericTypeDefinition = implementationGenericTypeDefinition;
                _serviceGenericTypeDefinitions = serviceGenericTypeDefinitions;
            }

            public void Register(Registrator registrator)
            {
                registrator.RegisterGeneric(_implementationGenericTypeDefinition, _serviceGenericTypeDefinitions);
            }
        }
    }
}