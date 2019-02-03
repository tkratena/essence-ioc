using System.Diagnostics.Contracts;

namespace Essence.Ioc.ExtendableRegistration
{
    public interface IContainer
    {
        [Pure]
        TService Resolve<TService>() where TService : class;
    }
}