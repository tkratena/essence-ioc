using System;
using System.Linq.Expressions;

namespace Essence.Ioc.Resolution
{
    internal static class FactoryExpression
    {
        public static IFactoryExpression Create(Expression body)
        {
            return new Value(body);
        }

        public static IFactoryExpression CreateLazy(Func<Expression> body)
        {
            return new Lazy(body);
        }
        
        private static Func<T> CompileLambda<T>(Expression body)
        {
            return Expression.Lambda<Func<T>>(body).Compile();
        }

        private class Value : IFactoryExpression
        {
            public Value(Expression body)
            {
                Body = body;
            }

            public Expression Body { get; }
            
            public Func<T> CompileFactory<T>()
            {
                return CompileLambda<T>(Body);
            }
        }
        
        private class Lazy : Lazy<Expression>, IFactoryExpression
        {
            public Lazy(Func<Expression> bodyProvider)
                : base(bodyProvider)
            {
            }

            public Expression Body => Value;

            public Func<T> CompileFactory<T>()
            {
                return CompileLambda<T>(Body);
            }
        }
    }
}