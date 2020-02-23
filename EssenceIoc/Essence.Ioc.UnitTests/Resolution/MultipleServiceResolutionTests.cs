using System.Diagnostics.CodeAnalysis;
using Essence.Ioc.FluentRegistration;
using NUnit.Framework;

namespace Essence.Ioc.Resolution
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class MultipleServiceResolutionTests
    {
        [Test]
        public void MultipleServicesImplementedByTheSameClass()
        {
            var container = new Container(r =>
                r.RegisterService<IService1>()
                    .AndService<IService2>()
                    .AndService<IService3>()
                    .AndService<IService4>()
                    .AndService<IService5>()
                    .AndService<IService6>()
                    .AndService<IService7>()
                    .AndService<IService8>()
                    .AndService<IService9>()
                    .AndService<IService10>()
                    .ImplementedBy<MultipleServiceImplementation>());

            container.Resolve<IService1>(out var s1);
            container.Resolve<IService2>(out var s2);
            container.Resolve<IService3>(out var s3);
            container.Resolve<IService4>(out var s4);
            container.Resolve<IService5>(out var s5);
            container.Resolve<IService6>(out var s6);
            container.Resolve<IService7>(out var s7);
            container.Resolve<IService8>(out var s8);
            container.Resolve<IService9>(out var s9);
            container.Resolve<IService10>(out var s10);

            Assert.That(
                new object[] {s1, s2, s3, s4, s5, s6, s7, s8, s9, s10},
                Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }

        [Test]
        public void MultipleServicesConstructedByTheSameFactory()
        {
            var container = new Container(r =>
                r.RegisterService<IService1>()
                    .AndService<IService2>()
                    .AndService<IService3>()
                    .ConstructedBy(() => new MultipleServiceImplementation()));

            container.Resolve<IService1>(out var service1);
            container.Resolve<IService2>(out var service2);
            container.Resolve<IService3>(out var service3);

            Assert.That(
                new object[] {service1, service2, service3},
                Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }

        [Test]
        public void MultipleGenericallyRegisteredServicesImplementedByTheSameClass()
        {
            var container = new Container(r =>
                r.GenericallyRegisterService(typeof(IGenericService1<>))
                    .AndService(typeof(IGenericService2<>))
                    .ImplementedBy(typeof(MultipleGenericServiceImplementation<>)));

            container.Resolve<IGenericService1<IActualGenericArgument>>(out var service1);
            container.Resolve<IGenericService2<IActualGenericArgument>>(out var service2);
            container.Resolve<IGenericService2<IActualGenericArgument>>(out var service3);

            Assert.That(
                new object[] {service1, service2, service3},
                Is.Unique.And.All.InstanceOf<MultipleGenericServiceImplementation<IActualGenericArgument>>());
        }

        private class MultipleServiceImplementation :
            IService1,
            IService2,
            IService3,
            IService4,
            IService5,
            IService6,
            IService7,
            IService8,
            IService9,
            IService10
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

        private interface IService9
        {
        }

        private interface IService10
        {
        }

        private class MultipleGenericServiceImplementation<T> : IGenericService1<T>, IGenericService2<T>
        {
        }

        private interface IGenericService1<T>
        {
        }

        private interface IGenericService2<T>
        {
        }

        private interface IActualGenericArgument
        {
        }
    }
}