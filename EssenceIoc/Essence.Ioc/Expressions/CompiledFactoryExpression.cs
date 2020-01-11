using System;
using System.Linq.Expressions;
using Essence.Ioc.LifeCycleManagement;

namespace Essence.Ioc.Expressions
{
    internal class CompiledFactoryExpression : IFactoryExpression
    {
        private readonly Func<ILifeScope, object> _factory;
        private readonly Type _constructedType;

        public CompiledFactoryExpression(Func<ILifeScope, object> factory, Type constructedType)
        {
            _factory = factory;
            _constructedType = constructedType;
        }

        public Expression GetBody(Expression lifeScope) =>
            Expression.Convert(Expression.Invoke(Expression.Constant(_factory), lifeScope), _constructedType);

        public Func<ILifeScope, T> Compile<T>()
        {
            return lifeScope => (T) _factory.Invoke(lifeScope);
        }
    }
}