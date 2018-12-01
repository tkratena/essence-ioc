using System.Linq.Expressions;

namespace Essence.Ioc.Expressions
{
    internal interface IFactoryExpression
    {
        Expression Body { get; }
    }
}