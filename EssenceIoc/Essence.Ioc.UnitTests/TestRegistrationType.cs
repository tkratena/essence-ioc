using System;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.FluentRegistration;

namespace Essence.Ioc
{
    public class TestRegistrationType : TestCase
    {
        private readonly Func<Registerer, ILifeStyle> _registerServices;

        public TestRegistrationType(string description, Func<Registerer, ILifeStyle> registration)
            : base(description)
        {
            _registerServices = registration ?? throw new ArgumentNullException(nameof(registration));
        }

        public ILifeStyle Invoke(Registerer registerer) => _registerServices.Invoke(registerer);
    }
}