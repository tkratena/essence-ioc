using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Essence.Framework.System;
using Essence.Ioc.Expressions;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.TypeModel
{
    internal sealed class ServiceFactory
    {
        private static readonly MethodInfo CreateNestedScopeMethod = 
            typeof(ILifeScope).GetTypeInfo().GetMethod(nameof(ILifeScope.CreateNestedScope));
        
        private readonly IDelegateInfo _delegateInfo;

        public ServiceFactory(IDelegateInfo delegateInfo)
        {
            _delegateInfo = delegateInfo;
        }

        public IFactoryExpression Resolve(IFactoryFinder factoryFinder)
        {
            var constructedType = _delegateInfo.InvokeMethod.ReturnType;
            if (constructedType == typeof(void) || _delegateInfo.InvokeMethod.GetParameters().Any())
            {
                throw new NonFactoryDelegateException(_delegateInfo);
            }

            var service = new Service(constructedType).Resolve(factoryFinder);

            return new FactoryExpression(lifeScope =>
            {
                var closure = new Closure();
                var nestedScopeClosure = Expression.Property(Expression.Constant(closure), nameof(Closure.LifeScope));
                    
                var body = service.GetBody(nestedScopeClosure);
                var factory = Expression.Lambda(_delegateInfo.Type, body).Compile();

                var returnTarget = Expression.Label(_delegateInfo.Type);
                return Expression.Block(
                    Expression.Assign(nestedScopeClosure, Expression.Call(lifeScope, CreateNestedScopeMethod)),
                    Expression.Label(returnTarget, Expression.Constant(factory)));
            });
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Used by the expression")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local", Justification = "Used by the expression")]
        private class Closure
        {
            public ILifeScope LifeScope { get; set; }
        }
    }
}