using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Essence.Framework.System;
using Essence.Ioc.Expressions;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.TypeModel;

namespace Essence.Ioc.Resolution
{
    internal class Resolver
    {
        private readonly IDictionary<Type, Delegate> _compiledFactories = new Dictionary<Type, Delegate>();
        private readonly IFactoryFinder _factoryFinder;

        public Resolver(IFactoryFinder factoryFinder)
        {
            _factoryFinder = factoryFinder;
        }

        [Pure]
        public TService Resolve<TService>(ILifeScope lifeScope) where TService : class
        {
            return GetCompiledFactory<TService>().Invoke(lifeScope);
        }

        private Func<ILifeScope, TService> GetCompiledFactory<TService>()
        {
            var serviceType = typeof(TService);
            lock (serviceType)
            {
                if (_compiledFactories.TryGetValue(serviceType, out var factory))
                {
                    return CastFactory<TService>(factory);
                }

                var factoryExpression = GetFactoryExpression(serviceType);
                factory = factoryExpression.Compile<TService>();
                _compiledFactories[serviceType] = factory;

                return (Func<ILifeScope, TService>) factory;
            }
        }

        private static Func<ILifeScope, T> CastFactory<T>(Delegate sourceDelegate)
        {
            if (sourceDelegate is Func<ILifeScope, T> targetDelegate)
            {
                return targetDelegate;
            }

            var function = (Func<ILifeScope, object>) sourceDelegate;
            return lifeScope => (T)function.Invoke(lifeScope);
        }

        private IFactoryExpression GetFactoryExpression(Type serviceType)
        {
            if (_factoryFinder.TryGetFactory(serviceType, out var factoryExpression))
            {
                return factoryExpression;
            }
            
            var delegateInfo = serviceType.AsDelegate();
            if (delegateInfo != null)
            {
                return new ServiceFactory(delegateInfo).Resolve(_factoryFinder);
            }

            if (serviceType.GetTypeInfo().IsGenericType)
            {
                if (typeof(Lazy<>) == serviceType.GetGenericTypeDefinition())
                {
                    return new LazyService(serviceType).Resolve(_factoryFinder);
                }
                
                return new GenericService(serviceType).Resolve(_factoryFinder);
            }

            throw new NotRegisteredServiceException(serviceType);
        }
    }
}