using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Essence.Ioc.ExtendableRegistration;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    public class ThreadSafetyTests
    {
        [Test]
        public void ConcurrentResolution()
        {
            const int tryCount = 100000;
            const int parallelResolutionCount = 8;

            for (var j = 0; j < tryCount; j++)
            {
                var container = new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<ServiceImplementation>());

                var services = new IService[parallelResolutionCount];
                Parallel.For(0, parallelResolutionCount, i => services[i] = container.Resolve<IService>());

                Assert.That(services, Is.All.InstanceOf<ServiceImplementation>());
            }
        }

        [Test]
        public void ConcurrentRegistration()
        {
            const int tryCount = 10000;
            var parallelRegistrations = new Action<Registerer>[]
            {
                r => r.RegisterService<IService1>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService2>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService3>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService4>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService5>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService6>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService7>().ImplementedBy<ServiceImplementation>(),
                r => r.RegisterService<IService8>().ImplementedBy<ServiceImplementation>()
            };

            for (var j = 0; j < tryCount; j++)
            {
                var container = new Container(r => 
                    Parallel.ForEach(parallelRegistrations, registration => registration.Invoke(r)));

                var services = new object[]
                {
                    container.Resolve<IService1>(),
                    container.Resolve<IService2>(),
                    container.Resolve<IService3>(),
                    container.Resolve<IService4>(),
                    container.Resolve<IService5>(),
                    container.Resolve<IService6>(),
                    container.Resolve<IService7>(),
                    container.Resolve<IService8>()
                };

                Assert.That(services, Is.All.InstanceOf<ServiceImplementation>());
            }
        }

        private class ServiceImplementation
            : IService, IService1, IService2, IService3, IService4, IService5, IService6, IService7, IService8
        {
        }

        private interface IService
        {
        }

        private interface IService1
        {
        }

        private interface IService2
        {
        }

        private interface IService3
        {
        }

        private interface IService4
        {
        }

        private interface IService5
        {
        }

        private interface IService6
        {
        }

        private interface IService7
        {
        }

        private interface IService8
        {
        }
    }
}