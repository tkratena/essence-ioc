using System;
using System.Linq.Expressions;
using Essence.Ioc.LifeCycleManagement;

namespace Essence.Ioc.Expressions
{
    internal interface IFactoryExpression
    {
        Expression GetBody(Expression lifeScope);
        Func<ILifeScope, TService> Compile<TService>();
    }
}