using System;
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
            var singletonFactory = MakeSingleton(factoryExpression);
            return new CompiledFactoryExpression(singletonFactory, constructedType);
        }

        private Func<ILifeScope, object> MakeSingleton(IFactoryExpression factoryExpression)
        {
            var lifeScope = _singletonLifeScope.CreateNestedScope();
            var singleton = new Lazy<object>(() => factoryExpression.Compile<object>().Invoke(lifeScope));
            return transientLifeScope => singleton.Value;
        }
    }
}