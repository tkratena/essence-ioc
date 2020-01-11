using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.Resolution;

namespace Essence.Ioc.LifeCycleManagement
{
    internal class LifeScopedResolver : IContainer
    {
        private readonly ILifeScope _lifeScope;
        private readonly Resolver _resolver;

        public LifeScopedResolver(ILifeScope lifeScope, Resolver resolver)
        {
            _lifeScope = lifeScope;
            _resolver = resolver;
        }

        public TService Resolve<TService>() where TService : class
        {
            return _resolver.Resolve<TService>(_lifeScope);
        }
    }
}