using System;
using System.Diagnostics.Contracts;
using Essence.Ioc.FluentRegistration;
using Essence.Ioc.Registration;
using Essence.Ioc.Resolution;

namespace Essence.Ioc
{
    public class Container : IContainer
    {
        private readonly IContainer _resolver;

        public Container(Action<IRegistrator> serviceRegistration)
        {
            var factories = new Factories();
            _resolver = new Resolver(factories);
            
            var fluentRegistrator = new FluentRegistrator();
            serviceRegistration.Invoke(fluentRegistrator);
            
            var registrator = new Registrator(factories, _resolver);
            fluentRegistrator.ExecuteRegistration(registrator);
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            return _resolver.Resolve<TService>();
        }
    }
}