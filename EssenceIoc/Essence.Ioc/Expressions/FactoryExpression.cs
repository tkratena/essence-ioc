﻿using System;
using System.Linq.Expressions;
using Essence.Ioc.LifeCycleManagement;

namespace Essence.Ioc.Expressions
{
    internal class FactoryExpression : IFactoryExpression
    {
        public delegate Expression FactoryBodyProvider(Expression lifeScope);

        private readonly FactoryBodyProvider _bodyProvider;

        public FactoryExpression(FactoryBodyProvider bodyProvider)
        {
            _bodyProvider = bodyProvider;
        }

        public Expression GetBody(Expression lifeScope) => _bodyProvider.Invoke(lifeScope);

        public Func<ILifeScope, TService> Compile<TService>()
        {
            var lifeScope = Expression.Parameter(typeof(ILifeScope), "lifeScope");
            return Expression.Lambda<Func<ILifeScope, TService>>(GetBody(lifeScope), lifeScope).Compile();
        }
    }
}