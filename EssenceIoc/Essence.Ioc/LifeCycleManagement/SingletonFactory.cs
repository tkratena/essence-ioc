using System;
using System.Threading;
using Essence.Ioc.Expressions;

namespace Essence.Ioc.LifeCycleManagement
{
    internal class SingletonFactory
    {
        private readonly ILifeScope _singletonLifeScope;

        public SingletonFactory(ILifeScope singletonLifeScope)
        {
            _singletonLifeScope = singletonLifeScope;
        }

        public IFactoryExpression MakeSingleton(IFactoryExpression factoryExpression, Type constructedType)
        {
            var singletonFactoryExpression = MakeSingleton(factoryExpression);
            return new CompiledFactoryExpression(singletonFactoryExpression, constructedType);
        }

        private Func<ILifeScope, object> MakeSingleton(IFactoryExpression factoryExpression)
        {
            var lifeScope = _singletonLifeScope.CreateNestedScope();
            Func<object> singletonFactory = () => factoryExpression.Compile<object>().Invoke(lifeScope);
            var singleton = new LazyWithoutExceptionCaching<object>(singletonFactory);
            return transientLifeScope => singleton.Value;
        }

        private class LazyWithoutExceptionCaching<T>
        {
            private readonly Func<T> _factory;
            private T _value;
            private bool _initialized;
            private object _lock;

            public LazyWithoutExceptionCaching(Func<T> factory)
            {
                _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public T Value => LazyInitializer.EnsureInitialized(ref _value, ref _initialized, ref _lock, _factory);
        }
    }
}