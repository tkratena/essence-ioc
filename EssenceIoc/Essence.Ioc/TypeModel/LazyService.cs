using System;
using System.Linq.Expressions;
using System.Reflection;
using Essence.Framework.System;
using Essence.Ioc.Expressions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.TypeModel
{
    internal sealed class LazyService
    {
        private readonly Type _lazyType;

        public LazyService(Type lazyType)
        {
            _lazyType = lazyType;
        }

        public IFactoryExpression Resolve(IFactoryFinder factoryFinder)
        {
            var serviceType = _lazyType.GenericTypeArguments[0];
            var serviceFactoryDelegateInfo = typeof(Func<>).MakeGenericType(serviceType).AsDelegate();

            var factory = new ServiceFactory(serviceFactoryDelegateInfo).Resolve(factoryFinder);

            return FactoryExpression.CreateLazy(() =>
            {
                var constructor = _lazyType.GetTypeInfo().GetConstructor(new[] {serviceFactoryDelegateInfo.Type});

                // ReSharper disable once AssignNullToNotNullAttribute
                return Expression.New(constructor, factory.Body);
            });
        }
    }
}