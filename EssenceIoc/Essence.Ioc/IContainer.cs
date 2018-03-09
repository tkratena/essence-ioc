using System.Diagnostics.Contracts;

namespace Essence.Ioc
{
    public interface IContainer
    {
        [Pure]
        TService Resolve<TService>() where TService : class;
    }
}