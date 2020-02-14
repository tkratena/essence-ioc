using System;
using Essence.Ioc.Expressions;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.Registration;
using Essence.Ioc.Resolution;
using Essence.Ioc.TypeModel;

namespace Essence.Ioc.LifeCycleManagement
{
    internal class SingletonFactory
    {
        private readonly Factories _factories;
        private readonly Resolver _resolver;
        private readonly ILifeScope _singletonLifeScope;

        public SingletonFactory(Factories factories, Resolver resolver, ILifeScope singletonLifeScope)
        {
            _factories = factories;
            _resolver = resolver;
            _singletonLifeScope = singletonLifeScope;
        }

        public Func<ILifeScope, object> Resolve(Func<IContainer, object> factory)
        {
            var container = new LifeScopedResolver(_singletonLifeScope, _resolver);
            return Resolve(() => factory.Invoke(container));
        }

        public Func<ILifeScope, object> Resolve(Func<object> factory)
        {
            var factoryExpression = new CompiledFactoryExpression(factory.ConstructWithTracking, typeof(object));
            return MakeSingleton(factoryExpression);
        }

        public Func<ILifeScope, object> Resolve(Type implementationType)
        {
            var factoryExpression = new Implementation(implementationType).Resolve(_factories);
            return MakeSingleton(factoryExpression);
        }

        private Func<ILifeScope, object> MakeSingleton(IFactoryExpression factoryExpression)
        {
            var lifeScope = _singletonLifeScope.CreateNestedScope();
            var singleton = new Lazy<object>(() => factoryExpression.Compile<object>().Invoke(lifeScope));
            return transientLifeScope => singleton.Value;
        }
    }
}