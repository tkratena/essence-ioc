using System;
using System.Collections.Generic;

namespace Essence.Ioc.ExtendableRegistration
{
    public interface IBasicRegisterer
    {
        void RegisterTransient(Type implementationType, IEnumerable<Type> serviceTypes);
        void RegisterSingleton(Type implementationType, IEnumerable<Type> serviceTypes);

        void RegisterFactorySingleton<TImplementation>(
            Func<IContainer, TImplementation> factory, 
            IEnumerable<Type> serviceTypes)
            where TImplementation : class;

        void RegisterFactorySingleton<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class;

        void RegisterFactoryTransient<TImplementation>(
            Func<IContainer, TImplementation> factory, 
            IEnumerable<Type> serviceTypes)
            where TImplementation : class;

        void RegisterFactoryTransient<TImplementation>(
            Func<TImplementation> factory,
            IEnumerable<Type> serviceTypes)
            where TImplementation : class;

        void RegisterGeneric(
            Type implementationGenericTypeDefinition,
            IEnumerable<Type> serviceGenericTypeDefinitions);
    }
}