using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Essence.Framework.System
{
    [TestFixture]
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    public class SuperTypesTypeExtensionsTests
    {
        [Test]
        public void SuperTypesOfClass()
        {
            var type = typeof(Class);

            var superTypes = type.GetSuperTypes();
            
            CollectionAssert.AreEquivalent(
                new []
                {
                    typeof(IInterface),
                    typeof(IInterfaceA),
                    typeof(IInterfaceB),
                    typeof(IInterfaceImplementedMultipleTimes),
                    typeof(IInterfaceBParent),
                    typeof(Parent),
                    typeof(IParentInterface),
                    typeof(IParentInterfaceParent),
                    typeof(AbstractGrandParent),
                    typeof(IAbstractGrandParentInterface),
                    typeof(AbstractGreatGrandParent),
                    typeof(object)
                }, 
                superTypes);
        }
        
        [Test]
        public void SuperTypesOfInterface()
        {
            var type = typeof(IInterface);
            
            var superTypes = type.GetSuperTypes();
            
            CollectionAssert.AreEquivalent(
                new []
                {
                    typeof(IInterfaceA),
                    typeof(IInterfaceB),
                    typeof(IInterfaceImplementedMultipleTimes),
                    typeof(IInterfaceBParent)
                }, 
                superTypes);
        }

        private class Class : Parent, IInterface
        {
        }
        
        private interface IInterface : IInterfaceA, IInterfaceB, IInterfaceImplementedMultipleTimes
        {
        }

        private class Parent : AbstractGrandParent, IParentInterface, IInterfaceImplementedMultipleTimes
        {
        }

        private abstract class AbstractGrandParent : AbstractGreatGrandParent, IAbstractGrandParentInterface
        {
        }
        
        private abstract class AbstractGreatGrandParent
        {
        }

        private interface IAbstractGrandParentInterface : IInterfaceImplementedMultipleTimes
        {
        }

        private interface IParentInterface : IParentInterfaceParent
        {
        }

        private interface IParentInterfaceParent
        {
        }

        private interface IInterfaceA : IInterfaceImplementedMultipleTimes
        {
        }
        
        private interface IInterfaceB : IInterfaceBParent
        {
        }

        private interface IInterfaceBParent : IInterfaceImplementedMultipleTimes
        {
        }

        private interface IInterfaceImplementedMultipleTimes
        {
        }
        
        [Test]
        public void SuperTypesOfStruct()
        {
            var type = typeof(Struct);
            
            var superTypes = type.GetSuperTypes();
            
            CollectionAssert.AreEquivalent(
                new []
                {
                    typeof(IStructInterfaceA),
                    typeof(IStructInterfaceB),
                    typeof(IStructInterfaceAParent),
                    typeof(IStructInterfaceAGrandParent),
                    typeof(IInterfaceImplementedMultipleTimes),
                    typeof(ValueType),
                    typeof(object)
                }, 
                superTypes);
        }
        
        private struct Struct : IStructInterfaceA, IStructInterfaceB, IInterfaceImplementedMultipleTimes
        {
        }

        private interface IStructInterfaceA : IStructInterfaceAParent
        {
        }

        private interface IStructInterfaceAParent : IStructInterfaceAGrandParent, IInterfaceImplementedMultipleTimes
        {
        }

        private interface IStructInterfaceAGrandParent
        {
        }

        private interface IStructInterfaceB
        {
        }
    }
}