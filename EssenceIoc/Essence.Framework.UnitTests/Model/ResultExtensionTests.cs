using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Essence.Framework.Model
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ResultExtensionsTests
    {
        [Test]
        public void ValueOfSuccess()
        {
            var expectedValue = new Value();
            Result<Value, Error> success = expectedValue; // implicit conversion

            var value = success.ValueOrThrow();

            Assert.AreSame(expectedValue, value);
        }

        [Test]
        public void ErrorOfSuccessThrows()
        {
            var value = new Value();
            Result<Value, Error> success = value; // implicit conversion

            TestDelegate when = () => success.ErrorOrThrow();

            Assert.Catch<Exception>(when);
        }

        [Test]
        public void ErrorOfFailure()
        {
            var expectedError = new Error();
            Result<Value, Error> failure = expectedError; // implicit conversion

            var error = failure.ErrorOrThrow();

            Assert.AreSame(expectedError, error);
        }

        [Test]
        public void ValueOfFailureThrows()
        {
            var error = new Error();
            Result<Value, Error> failure = error; // implicit conversion

            TestDelegate when = () => failure.ValueOrThrow();

            Assert.Catch<Exception>(when);
        }

        private class Value
        {
        }

        private class Error
        {
        }
    }
}