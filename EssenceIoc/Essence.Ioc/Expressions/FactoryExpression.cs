using System;
using System.Linq.Expressions;

namespace Essence.Ioc.Expressions
{
    internal static class FactoryExpression
    {
        public static IFactoryExpression CreateLazy(Func<Expression> body)
        {
            return new Lazy(body);
        }
        
        public static IFactoryExpression CreateCompiled<T>(Func<T> factory, Type serviceType)
        {
            return new Compiled(factory, serviceType);
        }

        private class Lazy : Lazy<Expression>, IFactoryExpression
        {
            public Lazy(Func<Expression> bodyProvider)
                : base(bodyProvider)
            {
            }

            public Expression Body => Value;
            
            public Func<T> Compile<T>() => CompileLambda<T>(Body);

            private static Func<T> CompileLambda<T>(Expression body)
            {
                return Expression.Lambda<Func<T>>(body).Compile();
            }
        }

        private class Compiled : IFactoryExpression
        {
            private readonly Func<object> _factory;
            private readonly Type _constructedType;

            public Compiled(Delegate factory, Type constructedType)
            {
                _factory = (Func<object>)factory;
                _constructedType = constructedType;
            }

            public Expression Body => 
                Expression.Convert(Expression.Invoke(Expression.Constant(_factory)), _constructedType);

            public Func<T> Compile<T>()
            {
                return () => (T) _factory.Invoke();
            }
        }
    }
}