using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Essence.Framework;
using Essence.Ioc.Expressions;
using Essence.Ioc.Registration.RegistrationExceptions;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.TypeModel
{
    internal sealed class Implementation
    {
        private readonly Type _type;

        public Implementation(Type implementationType)
        {
            _type = implementationType;
        }

        public IFactoryExpression Resolve(IFactoryFinder factoryFinder)
        {
            if (!IsConcreteClass(_type))
            {
                throw new NonConcreteClassException(_type);
            }

            if (typeof(IDisposable).GetTypeInfo().IsAssignableFrom(_type))
            {
                throw new DisposableClassException(_type);
            }

            var constructor = GetSinglePublicConstructor(_type);
            var resolvedDependencies = ResolveDependencies(constructor, factoryFinder).ToList();

            return FactoryExpression.CreateLazy(
                () => Expression.New(constructor, resolvedDependencies.Select(d => d.Body)));
        }

        private static bool IsConcreteClass(Type implementationType)
        {
            return implementationType.GetTypeInfo().IsClass && !implementationType.GetTypeInfo().IsAbstract;
        }

        private static ConstructorInfo GetSinglePublicConstructor(Type classType)
        {
            var publicConstructors = classType.GetTypeInfo().GetConstructors();

            if (publicConstructors.Length == 0)
            {
                throw new NoConstructorException(classType);
            }

            if (publicConstructors.Length > 1)
            {
                throw new AmbiguousConstructorsException(classType);
            }

            return publicConstructors.Single();
        }

        private IEnumerable<IFactoryExpression> ResolveDependencies(
            ConstructorInfo constructor,
            IFactoryFinder factoryFinder)
        {
            return GetDependencies(constructor).Select(d => ResolveDependency(d, factoryFinder));
        }

        private IEnumerable<Type> GetDependencies(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            var unsupportedParameters = parameters
                .Where(p => p.Attributes != ParameterAttributes.None || p.ParameterType.IsByRef)
                .ToList();

            if (unsupportedParameters.Any())
            {
                throw new UnsupportedConstructorParametersException(_type);
            }

            return parameters.Select(p => p.ParameterType);
        }

        private IFactoryExpression ResolveDependency(Type type, IFactoryFinder factoryFinder)
        {
            try
            {
                if (type.GetTypeInfo().IsGenericType && typeof(Lazy<>) == type.GetGenericTypeDefinition())
                {
                    return new LazyService(type).Resolve(factoryFinder);
                }

                var delegateInfo = type.AsDelegate();
                if (delegateInfo != null)
                {
                    return new ServiceFactory(delegateInfo).Resolve(factoryFinder);
                }

                return new Service(type).Resolve(factoryFinder);
            }
            catch (RegistrationException e)
            {
                throw new DependencyRegistrationException(_type, e);
            }
        }
    }
}