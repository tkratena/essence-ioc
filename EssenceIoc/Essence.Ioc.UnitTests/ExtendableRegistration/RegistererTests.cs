using System.Collections.Generic;
using NUnit.Framework;

namespace Essence.Ioc.ExtendableRegistration
{
    [TestFixture]
    public class RegistererTests
    {
        [Test]
        public void AddRegistrationPassesRegistrationToRegisterer()
        {
            var spyRegisterer = new SpyRegisterer();
            var registrationInstance = new DummyRegistration();
            
            Registerer.AddRegistration(spyRegisterer, registrationInstance);
            
            Assert.That(spyRegisterer.AddedRegistrations, Is.EqualTo(new[] { registrationInstance }));
        }

        private class SpyRegisterer : Registerer 
        {
            private readonly List<IRegistration> _addedRegistrations = new List<IRegistration>();

            public IReadOnlyList<IRegistration> AddedRegistrations => _addedRegistrations;
            
            protected override void AddRegistration(IRegistration r) => _addedRegistrations.Add(r);
        }

        private class DummyRegistration : IRegistration
        {
            public void Register(IRegisterer registerer)
            {
            }
        }
    }
}