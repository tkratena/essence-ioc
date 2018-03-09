using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Ioc.TypeModel;

namespace Essence.Ioc.Resolution
{
    internal class Resolver : IContainer
    {
        private readonly IDictionary<Type, Delegate> _compiledFactories = new Dictionary<Type, Delegate>();
        private readonly IFactoryFinder _factoryFinder;

        public Resolver(IFactoryFinder factoryFinder)
        {
            _factoryFinder = factoryFinder;
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            return GetCompiledFactory<TService>().Invoke();
        }

        private Func<TService> GetCompiledFactory<TService>()
        {
            var serviceType = typeof(TService);

            if (_compiledFactories.TryGetValue(serviceType, out var factory)
                || _factoryFinder.TryGetFactory(serviceType, out factory))
            {
                return Cast<TService>(factory);
            }

            var factoryExpression = GetFactoryExpression(serviceType);
            factory = factoryExpression.CompileFactory<TService>();
            _compiledFactories[serviceType] = factory;

            return (Func<TService>) factory;
        }

        private Func<T> Cast<T>(Delegate factory)
        {
            return factory as Func<T> ?? (() => (T)((Func<object>)factory).Invoke());
        }

        private IFactoryExpression GetFactoryExpression(Type serviceType)
        {
            if (_factoryFinder.TryGetFactoryExpression(serviceType, out var expression))
            {
                return expression;
            }

            if (serviceType.IsGenericType)
            {
                return new GenericService(serviceType).Resolve(_factoryFinder);
            }

            throw new NotRegisteredServiceException(serviceType);
        }
    }
}