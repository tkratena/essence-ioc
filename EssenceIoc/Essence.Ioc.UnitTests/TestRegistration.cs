using System;
using Essence.Ioc.ExtendableRegistration;

namespace Essence.Ioc
{
    public class TestRegistration : TestCase
    {
        private readonly Action<Registerer> _registerServices;

        public TestRegistration(string description, Action<Registerer> registration) 
            : base(description)
        {
            _registerServices = registration ?? throw new ArgumentNullException(nameof(registration));
        }

        public void Invoke(Registerer registerer) => _registerServices.Invoke(registerer);
    }
}