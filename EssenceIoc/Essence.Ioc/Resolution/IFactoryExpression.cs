using System.Linq.Expressions;

namespace Essence.Ioc.Resolution
{
    internal interface IFactoryExpression
    {
        Expression Body { get; }
    }
}