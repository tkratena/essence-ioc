using System.Diagnostics.CodeAnalysis;
using Essence.Framework.Model;
using NUnit.Framework;

namespace Essence.Framework
{
    [TestFixture]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public class ResultTests
    {
        [TestFixture]
        public class ResultWithoutValue
        {
            [Test]
            public void ResultCannotBeNull()
            {
                var result = default(Result);

                Assert.IsNotNull(result);
            }

            [Test]
            public void Success()
            {
                Result success = Result.Success();

                var isSuccess = success.Case(success: () => true, failure: () => false);
                Assert.IsTrue(isSuccess);

                success.Case(
                    success: Assert.Pass,
                    failure: () => Assert.Fail("Wrong delegate called"));

                Assert.Fail("No delegate called");
            }

            [Test]
            public void Failure()
            {
                Result failure = Result.Failure(); // implicit conversion

                var isFailure = failure.Case(success: () => false, failure: () => true);
                Assert.IsTrue(isFailure);

                failure.Case(
                    success: () => Assert.Fail("Wrong delegate called"),
                    failure: Assert.Pass);

                Assert.Fail("No delegate called");
            }
            
            [Test]
            public void SuccessesAreEqual()
            {
                Result success1 = Result.Success();
                Result success2 = Result.Success();
                
                Assert.AreEqual(success1, success2);
                Assert.AreEqual(success1.GetHashCode(), success2.GetHashCode());
            }
            
            [Test]
            public void FailuresAreEqual()
            {
                Result failure1 = Result.Failure(); // implicit conversion
                Result failure2 = Result.Failure(); // implicit conversion
                
                Assert.AreEqual(failure1, failure2);
                Assert.AreEqual(failure1.GetHashCode(), failure2.GetHashCode());
            }
            
            [Test]
            public void SuccessAndFailureAreNotEqual()
            {
                Result success = Result.Success();
                Result failure = Result.Failure(); // implicit conversion
                
                Assert.AreNotEqual(success, failure);
                Assert.AreNotEqual(success.GetHashCode(), failure.GetHashCode());
            }
        }
        
        [TestFixture]
        public class ResultWithValue
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
                Result<ValueObject> success = expectedValue; // implicit conversion

                var returnedValue = success.Case(success: value => value, failure: () => null);
                Assert.AreSame(expectedValue, returnedValue);

                success.Case(
                    success: value =>
                    {
                        Assert.AreSame(expectedValue, value);
                        Assert.Pass();
                    },
                    failure: () => Assert.Fail("Wrong delegate called"));

                Assert.Fail("No delegate called");
            }

            [Test]
            public void Failure()
            {
                Result<ValueObject> failure = Result.Failure(); // implicit conversion

                var isError = failure.Case(success: value => false, failure: () => true);
                Assert.IsTrue(isError);

                failure.Case(
                    success: value => Assert.Fail("Wrong delegate called"),
                    failure: Assert.Pass);

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
            public void ToStringOfNullSuccessDoesNotThrow()
            {
                Result<object> success = null; // implicit conversion

                TestDelegate when = () => success.ToString();

                Assert.DoesNotThrow(when);
            }
            
            [Test]
            public void SuccessesWithEqualValuesAreEqual()
            {
                Result<string> success1 = "value"; // implicit conversion
                Result<string> success2 = "value"; // implicit conversion
                
                Assert.AreEqual(success1, success2);
                Assert.AreEqual(success1.GetHashCode(), success2.GetHashCode());
            }
            
            [Test]
            public void SuccessesWithDifferentValuesAreNotEqual()
            {
                Result<string> success1 = "value"; // implicit conversion
                Result<string> success2 = "different value"; // implicit conversion
                
                Assert.AreNotEqual(success1, success2);
                Assert.AreNotEqual(success1.GetHashCode(), success2.GetHashCode());
            }
            
            [Test]
            public void FailuresAreEqual()
            {
                Result<string> failure1 = Result.Failure(); // implicit conversion
                Result<string> failure2 = Result.Failure(); // implicit conversion
                
                Assert.AreEqual(failure1, failure2);
                Assert.AreEqual(failure1.GetHashCode(), failure2.GetHashCode());
            }
            
            [Test]
            public void SuccessAndFailureAreNotEqual()
            {
                Result<string> success = null; // implicit conversion
                Result<string> failure = Result.Failure(); // implicit conversion
                
                Assert.AreNotEqual(success, failure);
                Assert.AreNotEqual(success.GetHashCode(), failure.GetHashCode());
            }
        }

        [TestFixture]
        public class ResultWithValueAndError
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