using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Essence.Framework
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ResultTests
    {
        [TestFixture]
        public class DifferentValueAndErrorTypes
        {
            [Test]
            public void ResultCannotBeNull()
            {
                var result = default(Result<ValueObject, ErrorStructure>);

                Assert.IsNotNull(result);
            }

            [Test]
            public void Success()
            {
                var expectedValue = new ValueObject();
                Result<ValueObject, ErrorStructure> success = expectedValue; // implicit conversion

                var returnedValue = success.Case(success: value => value, failure: error => null);
                Assert.AreSame(expectedValue, returnedValue);

                success.Case(
                    success: value =>
                    {
                        Assert.AreSame(expectedValue, value);
                        Assert.Pass();
                    },
                    failure: error => Assert.Fail("Wrong delegate called"));

                Assert.Fail("No delegate called");
            }

            [Test]
            public void Failure()
            {
                var expectedError = new ErrorStructure("error");
                Result<ValueObject, ErrorStructure> failure = expectedError; // implicit conversion

                var returnedError = failure.Case(success: value => default(ErrorStructure), failure: error => error);
                Assert.AreEqual(expectedError.Content, returnedError.Content);

                failure.Case(
                    success: value => Assert.Fail("Wrong delegate called"),
                    failure: error =>
                    {
                        Assert.AreEqual(expectedError.Content, error.Content);
                        Assert.Pass();
                    });

                Assert.Fail("No delegate called");
            }

            [Test]
            public void ToStringOfSuccessContainsTheValue()
            {
                var valueString = "ToString() method result of value object";
                var value = new ToStringObjectStub(valueString);
                Result<ToStringObjectStub, DummyStructure> success = value; // implicit conversion

                var successString = success.ToString();

                StringAssert.Contains(valueString, successString);
            }

            [Test]
            public void ToStringOfFailureContainsTheError()
            {
                var errorString = "ToString() method result of error object";
                var error = new ToStringObjectStub(errorString);
                Result<DummyStructure, ToStringObjectStub> failure = error; // implicit conversion

                var failureString = failure.ToString();

                StringAssert.Contains(errorString, failureString);
            }

            [Test]
            public void ToStringOfNullSuccessDoesNotThrow()
            {
                Result<object, DummyStructure> success = null; // implicit conversion

                TestDelegate when = () => success.ToString();

                Assert.DoesNotThrow(when);
            }

            [Test]
            public void ToStringOfNullFailureDoesNotThrow()
            {
                Result<DummyStructure, object> failure = null; // implicit conversion

                TestDelegate when = () => failure.ToString();

                Assert.DoesNotThrow(when);
            }
        }

        [TestFixture]
        public class SameValueAndErrorTypes
        {
            [Test]
            public void ResultCannotBeNull()
            {
                var result = default(Result<ValueObject>);

                Assert.IsNotNull(result);
            }


            [Test]
            public void Success()
            {
                var expectedValue = new ValueObject();
                Result<ValueObject> success = Result.Success(expectedValue); // implicit conversion

                var returnedValue = success.Case(success: value => value, failure: error => null);
                Assert.AreSame(expectedValue, returnedValue);

                success.Case(
                    success: value =>
                    {
                        Assert.AreSame(expectedValue, value);
                        Assert.Pass();
                    },
                    failure: error => Assert.Fail("Wrong delegate called"));

                Assert.Fail("No delegate called");
            }

            [Test]
            public void Failure()
            {
                var expectedError = new ValueObject();
                Result<ValueObject> failure = Result.Failure(expectedError); // implicit conversion

                var returnedError = failure.Case(success: value => null, failure: error => error);
                Assert.AreSame(expectedError, returnedError);

                failure.Case(
                    success: value => Assert.Fail("Wrong delegate called"),
                    failure: error =>
                    {
                        Assert.AreSame(expectedError, error);
                        Assert.Pass();
                    });

                Assert.Fail("No delegate called");
            }


            [Test]
            public void ToStringOfSuccessContainsTheValue()
            {
                var valueString = "ToString() method result of value object";
                var value = new ToStringObjectStub(valueString);
                Result<ToStringObjectStub> success = Result.Success(value); // implicit conversion

                var successString = success.ToString();

                StringAssert.Contains(valueString, successString);
            }

            [Test]
            public void ToStringOfFailureContainsTheError()
            {
                var errorString = "ToString() method result of error object";
                var error = new ToStringObjectStub(errorString);
                Result<ToStringObjectStub> failure = Result.Failure(error); // implicit conversion

                var failureString = failure.ToString();

                StringAssert.Contains(errorString, failureString);
            }

            [Test]
            public void ToStringOfNullSuccessDoesNotThrow()
            {
                Result<object> success = Result.Success<object>(null); // implicit conversion

                TestDelegate when = () => success.ToString();

                Assert.DoesNotThrow(when);
            }

            [Test]
            public void ToStringOfNullFailureDoesNotThrow()
            {
                Result<object> failure = Result.Failure<object>(null); // implicit conversion

                TestDelegate when = () => failure.ToString();

                Assert.DoesNotThrow(when);
            }
        }

        private class ValueObject
        {
        }

        private struct ErrorStructure
        {
            public string Content { get; }

            public ErrorStructure(string content)
            {
                Content = content;
            }
        }

        private class ToStringObjectStub
        {
            private readonly string _toString;

            public ToStringObjectStub(string toString)
            {
                _toString = toString;
            }

            public override string ToString()
            {
                return _toString;
            }
        }

        private struct DummyStructure
        {
        }
    }
}