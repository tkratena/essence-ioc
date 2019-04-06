using System.Diagnostics.CodeAnalysis;
using Essence.Framework.Model;
using NUnit.Framework;

namespace Essence.Framework
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ResultTests
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
            Assert.AreEqual(expectedError, returnedError);

            failure.Case(
                success: value => Assert.Fail("Wrong delegate called"),
                failure: error =>
                {
                    Assert.AreEqual(expectedError, error);
                    Assert.Pass();
                });

            Assert.Fail("No delegate called");
        }
        
        [Test]
        public void ExplicitSuccess()
        {
            var expectedValue = new ValueObject();
            Result<ValueObject, ValueObject> success = Result.Success(expectedValue);

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
        public void ExplicitFailure()
        {
            var expectedError = new ValueObject();
            Result<ValueObject, ValueObject> failure = Result.Failure(expectedError); // implicit conversion

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
        public void SuccessesWithEqualValuesAreEqual()
        {
            Result<string, DummyStructure> success1 = "value"; // implicit conversion
            Result<string, DummyStructure> success2 = "value"; // implicit conversion

            Assert.AreEqual(success1, success2);
            Assert.AreEqual(success1.GetHashCode(), success2.GetHashCode());
        }

        [Test]
        public void SuccessesWithDifferentValuesAreNotEqual()
        {
            Result<string, DummyStructure> success1 = "value"; // implicit conversion
            Result<string, DummyStructure> success2 = "different value"; // implicit conversion

            Assert.AreNotEqual(success1, success2);
            Assert.AreNotEqual(success1.GetHashCode(), success2.GetHashCode());
        }

        [Test]
        public void FailuresWithEqualErrorsAreEqual()
        {
            Result<DummyStructure, string> failure1 = "error"; // implicit conversion
            Result<DummyStructure, string> failure2 = "error"; // implicit conversion

            Assert.AreEqual(failure1, failure2);
            Assert.AreEqual(failure1.GetHashCode(), failure2.GetHashCode());
        }

        [Test]
        public void FailuresWithDifferentErrorsAreNotEqual()
        {
            Result<DummyStructure, string> failure1 = "error"; // implicit conversion
            Result<DummyStructure, string> failure2 = "a different error"; // implicit conversion

            Assert.AreNotEqual(failure1, failure2);
            Assert.AreNotEqual(failure1.GetHashCode(), failure2.GetHashCode());
        }

        [Test]
        public void SuccessAndFailureWithEqualValueAndErrorAreNotEqual()
        {
            Result<string, string> success = Result.Success("value"); // implicit conversion
            Result<string, string> failure = Result.Failure("value"); // implicit conversion

            Assert.AreNotEqual(success, failure);
            Assert.AreNotEqual(success.GetHashCode(), failure.GetHashCode());
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

        private class ValueObject
        {
        }

        private struct ErrorStructure
        {
            [SuppressMessage("ReSharper", "NotAccessedField.Local")] 
            private readonly string _content;

            public ErrorStructure(string content)
            {
                _content = content;
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