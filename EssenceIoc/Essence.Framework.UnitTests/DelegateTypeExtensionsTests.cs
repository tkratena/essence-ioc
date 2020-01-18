using System;
using System.Linq;
using System.Reflection;
using Essence.Framework.System;
using NUnit.Framework;

namespace Essence.Framework
{
    [TestFixture]
    public class DelegateTypeExtensionsTests
    {
        [Test]
        public void Action()
        {
            var type = typeof(Action<IParameter>);

            var delegateInfo = type.AsDelegate();

            Assert.IsNotNull(delegateInfo);
            Assert.AreEqual(typeof(void), delegateInfo.InvokeMethod.ReturnType);
            CollectionAssert.AreEqual(
                new[] {typeof(IParameter)},
                delegateInfo.InvokeMethod.GetParameters().Select(p => p.ParameterType));
        }
        
        [Test]
        public void Func()
        {
            var type = typeof(Func<IParameter, IResult>);

            var delegateInfo = type.AsDelegate();

            Assert.IsNotNull(delegateInfo);
            Assert.AreEqual(typeof(IResult), delegateInfo.InvokeMethod.ReturnType);
            CollectionAssert.AreEqual(
                new[] {typeof(IParameter)},
                delegateInfo.InvokeMethod.GetParameters().Select(p => p.ParameterType));
        }
        
        [Test]
        public void Delegate()
        {
            var type = typeof(CustomDelegate);

            var delegateInfo = type.AsDelegate();

            Assert.IsNotNull(delegateInfo);
            Assert.AreEqual(typeof(IResult), delegateInfo.InvokeMethod.ReturnType);
            CollectionAssert.AreEqual(
                new[] {typeof(IParameter)},
                delegateInfo.InvokeMethod.GetParameters().Select(p => p.ParameterType));
        }
        
        private delegate IResult CustomDelegate(IParameter parameter);

        private interface IParameter
        {
        }

        private interface IResult
        {
        }
        
        [Test]
        public void ActionGenericDefinition()
        {
            var type = typeof(Action<>);

            var delegateInfo = type.AsDelegate();

            Assert.IsNotNull(delegateInfo);
            Assert.AreEqual(typeof(void), delegateInfo.InvokeMethod.ReturnType);
            CollectionAssert.AreEqual(
                new[] {type.GetTypeInfo().GenericTypeParameters[0]},
                delegateInfo.InvokeMethod.GetParameters().Select(p => p.ParameterType));
        }
        
        [Test]
        public void FuncGenericDefinition()
        {
            var type = typeof(Func<,>);

            var delegateInfo = type.AsDelegate();

            Assert.IsNotNull(delegateInfo);
            Assert.AreEqual(type.GetTypeInfo().GenericTypeParameters[1], delegateInfo.InvokeMethod.ReturnType);
            CollectionAssert.AreEqual(
                new[] {type.GetTypeInfo().GenericTypeParameters[0]},
                delegateInfo.InvokeMethod.GetParameters().Select(p => p.ParameterType));
        }
        
        [Test]
        [TestCase(typeof(NonDelegateClass))]
        [TestCase(typeof(INonDelegateInterface))]
        [TestCase(typeof(object))]
        [TestCase(typeof(string))]
        [TestCase(typeof(int))]
        public void NonDelegate(Type type)
        {
            var delegateInfo = type.AsDelegate();

            Assert.IsNull(delegateInfo);
        }

        private class NonDelegateClass
        {
        }
        
        private interface INonDelegateInterface
        {
        }
    }
}
