using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace Essence.Ioc
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public class MultipleServiceResolutionTests
    {
        [Test]
        public void MultipleServicesImplementedBySameClass()
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

            var resolvedServices = new object[]
            {
                container.Resolve<IService1>(),
                container.Resolve<IService2>(),
                container.Resolve<IService3>(),
                container.Resolve<IService4>(),
                container.Resolve<IService5>(),
                container.Resolve<IService6>(),
                container.Resolve<IService7>(),
                container.Resolve<IService8>(),
                container.Resolve<IService9>(),
                container.Resolve<IService10>()
            };
            
            Assert.That(resolvedServices, Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }
        
        [Test]
        public void MultipleServicesImplementedBySameClassAsSingleton()
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
                    .ImplementedBy<MultipleServiceImplementation>()
                    .AsSingleton());

            var resolvedServices = new object[]
            {
                container.Resolve<IService1>(),
                container.Resolve<IService2>(),
                container.Resolve<IService3>(),
                container.Resolve<IService4>(),
                container.Resolve<IService5>(),
                container.Resolve<IService6>(),
                container.Resolve<IService7>(),
                container.Resolve<IService8>(),
                container.Resolve<IService9>(),
                container.Resolve<IService10>()
            };

            var first = resolvedServices.First();
            Assert.That(resolvedServices, Is.All.InstanceOf<MultipleServiceImplementation>().And.SameAs(first));
        }
        
        [Test]
        public void MultipleServicesConstructedBySameFactory()
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
                    .ConstructedBy(() => new MultipleServiceImplementation()));

            var resolvedServices = new object[]
            {
                container.Resolve<IService1>(),
                container.Resolve<IService2>(),
                container.Resolve<IService3>(),
                container.Resolve<IService4>(),
                container.Resolve<IService5>(),
                container.Resolve<IService6>(),
                container.Resolve<IService7>(),
                container.Resolve<IService8>(),
                container.Resolve<IService9>(),
                container.Resolve<IService10>()
            };
            
            Assert.That(resolvedServices, Is.Unique.And.All.InstanceOf<MultipleServiceImplementation>());
        }
        
        [Test]
        public void MultipleServicesConstructedBySameFactoryAsSingleton()
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
                    .ConstructedBy(() => new MultipleServiceImplementation())
                    .AsSingleton());

            var resolvedServices = new object[]
            {
                container.Resolve<IService1>(),
                container.Resolve<IService2>(),
                container.Resolve<IService3>(),
                container.Resolve<IService4>(),
                container.Resolve<IService5>(),
                container.Resolve<IService6>(),
                container.Resolve<IService7>(),
                container.Resolve<IService8>(),
                container.Resolve<IService9>(),
                container.Resolve<IService10>()
            };

            var first = resolvedServices.First();
            Assert.That(resolvedServices, Is.All.InstanceOf<MultipleServiceImplementation>().And.SameAs(first));
        }
        
        [Test]
        public void MultipleGenericServicesImplementedBySameClass()
        {
            var container = new Container(r => 
                r.GenericlyRegisterService(typeof(IGenericService1<>))
                    .AndService(typeof(IGenericService2<>))
                    .ImplementedBy(typeof(MultipleGenericServiceImplementation<>)));

            var resolvedServices = new object[]
            {
                container.Resolve<IGenericService1<IActualGenericArgument>>(),
                container.Resolve<IGenericService2<IActualGenericArgument>>()
            };

            Assert.That(
                resolvedServices, 
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