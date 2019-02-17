using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
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

        private class ServiceImplementation : IService
        {
        }

        private interface IService
        {
        }
    }
}