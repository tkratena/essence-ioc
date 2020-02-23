using System;
using System.Collections.Generic;
using System.Linq;
using Essence.Framework.Linq;
using Essence.Ioc.ExtendableRegistration;

namespace Essence.Ioc.FluentRegistration
{
    internal class GenericService : IGenericServices
    {
        private readonly Registerer _registerer;
        private readonly IEnumerable<Type> _genericServiceTypeDefinitions;

        public GenericService(Registerer registerer, Type genericServiceTypeDefinition)
            : this(registerer, genericServiceTypeDefinition.UnfoldToEnumerable())
        {
        }
            
        private GenericService(
            Registerer registerer,
            IEnumerable<Type> genericServiceTypeDefinitions)
        {
            _registerer = registerer;
            _genericServiceTypeDefinitions = genericServiceTypeDefinitions;
        }
            
        public void ImplementedBy(Type genericServiceImplementationTypeDefinition)
        {
            var registration = new GenericImplementation(
                genericServiceImplementationTypeDefinition,
                _genericServiceTypeDefinitions);
            
            Registerer.AddRegistration(_registerer, registration);
        }

        public IGenericServices AndService(Type genericServiceTypeDefinition)
        {
            return new GenericService(
                _registerer, 
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

            public void Register(IRegisterer registerer)
            {
                registerer.RegisterGeneric(_implementationGenericTypeDefinition, _serviceGenericTypeDefinitions);
            }
        }
    }
}