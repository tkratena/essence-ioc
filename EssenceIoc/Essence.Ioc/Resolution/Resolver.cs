using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Essence.Framework.System;
using Essence.Ioc.Expressions;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.TypeModel;

namespace Essence.Ioc.Resolution
{
    internal class Resolver : IContainer
    {
        private readonly IDictionary<Type, Delegate> _compiledFactories = new Dictionary<Type, Delegate>();
        private readonly IFactoryFinder _factoryFinder;
        private readonly InstanceTracker _tracker;

        public Resolver(IFactoryFinder factoryFinder, InstanceTracker tracker)
        {
            _factoryFinder = factoryFinder;
            _tracker = tracker;
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            return GetCompiledFactory<TService>().Invoke();
        }

        private Func<TService> GetCompiledFactory<TService>()
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

                return (Func<TService>) factory;
            }
        }

        private static Func<T> CastFactory<T>(Delegate sourceDelegate)
        {
            if (sourceDelegate is Func<T> targetDelegate)
            {
                return targetDelegate;
            }

            var function = (Func<object>) sourceDelegate;
            return () => (T)function.Invoke();
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
                return new ServiceFactory(delegateInfo).Resolve(_factoryFinder, _tracker);
            }

            if (serviceType.GetTypeInfo().IsGenericType)
            {
                if (typeof(Lazy<>) == serviceType.GetGenericTypeDefinition())
                {
                    return new LazyService(serviceType).Resolve(_factoryFinder, _tracker);
                }
                
                return new GenericService(serviceType).Resolve(_factoryFinder, _tracker);
            }

            throw new NotRegisteredServiceException(serviceType);
        }
    }
}