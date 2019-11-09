using System;
using System.Linq;
using System.Linq.Expressions;
using Essence.Framework.System;
using Essence.Ioc.Expressions;
using Essence.Ioc.LifeCycleManagement;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.TypeModel
{
    internal sealed class ServiceFactory
    {
        private readonly IDelegateInfo _delegateInfo;

        public ServiceFactory(IDelegateInfo delegateInfo)
        {
            _delegateInfo = delegateInfo;
        }

        public IFactoryExpression Resolve(IFactoryFinder factoryFinder, InstanceTracker tracker)
        {
            var constructedType = _delegateInfo.InvokeMethod.ReturnType;
            if (constructedType == typeof(void) || _delegateInfo.InvokeMethod.GetParameters().Any())
            {
                throw new NonFactoryDelegateException(_delegateInfo);
            }

            var service = new Service(constructedType).Resolve(factoryFinder, tracker);
            return FactoryExpression.CreateLazy(() => CreateLambdaExpression(service.Body, _delegateInfo.Type));
        }

        private static Expression CreateLambdaExpression(Expression body, Type delegateType)
        {
            return Expression.Constant(Expression.Lambda(delegateType, body).Compile());
        }
    }
}