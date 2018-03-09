using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Essence.TestFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestCaseAttribute : NUnit.Framework.TestCaseAttribute, ITestBuilder
    {
        public TestCaseAttribute(params object[] arguments) : base(arguments)
        {
        }

        public Type[] Generics { get; set; }

        public Type Generic
        {
            get => Generics?.FirstOrDefault();
            set => Generics = new[] {value};
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test suite)
        {
            if (!method.IsGenericMethodDefinition)
            {
                return base.BuildFrom(method, suite);
            }

            try
            {
                var genericMethod = method.MakeGenericMethod(Generics.ToArray());
                return base.BuildFrom(genericMethod, suite);
            }
            catch (ArgumentException e)
            {
                var parameters = new TestCaseParameters {RunState = RunState.NotRunnable};
                parameters.Properties.Set("_SKIPREASON", e.Message);
                return new[] {new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters)};
            }
        }
    }
}