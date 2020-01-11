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
            const int tryCount = 100_000;
            const int parallelResolutionCount = 8;

            for (var j = 0; j < tryCount; j++)
            {
                var container = new Container(r =>
                    r.RegisterService<IService>().ImplementedBy<ServiceImplementation>());

                var services = new IService[parallelResolutionCount];
                Parallel.For(0, parallelResolutionCount, i => container.Resolve(out services[i]));

                Assert.That(services, Is.All.InstanceOf<ServiceImplementation>());
            }
        }

        [Test]
        public void ConcurrentRegistration()
        {
            const int tryCount = 10_000;
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

                container.Resolve<IService1>(out var s1);
                container.Resolve<IService2>(out var s2);
                container.Resolve<IService3>(out var s3);
                container.Resolve<IService4>(out var s4);
                container.Resolve<IService5>(out var s5);
                container.Resolve<IService6>(out var s6);
                container.Resolve<IService7>(out var s7);
                container.Resolve<IService8>(out var s8);

                Assert.That(
                    new object[] {s1, s2, s3, s4, s5, s6, s7, s8},
                    Is.All.InstanceOf<ServiceImplementation>());
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