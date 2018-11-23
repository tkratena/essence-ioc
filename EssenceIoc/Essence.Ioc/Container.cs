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

        public Container(Action<IRegisterer> serviceRegistration)
        {
            var factories = new Factories();
            _resolver = new Resolver(factories);
            
            var fluentRegisterer = new FluentRegisterer();
            serviceRegistration.Invoke(fluentRegisterer);
            
            var registerer = new Registerer(factories, _resolver);
            fluentRegisterer.ExecuteRegistration(registerer);
        }

        [Pure]
        public TService Resolve<TService>() where TService : class
        {
            return _resolver.Resolve<TService>();
        }
    }
}