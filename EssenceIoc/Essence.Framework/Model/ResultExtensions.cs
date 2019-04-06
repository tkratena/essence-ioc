using System;
using System.Diagnostics.Contracts;

namespace Essence.Framework.Model
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Returns the value when the result is a success. When it is a failure, throws.<para/>
        /// To be able to process both cases, use
        /// <see cref="Result{TValue,TError}.Case(Action{TValue},Action{TError})"/> or
        /// <see cref="Result{TValue,TError}.Case{T}(Func{TValue,T},Func{TError,T})"/>.
        /// </summary>
        /// <returns>The value when the result is a success</returns>
        /// <exception cref="Exception">Thrown when the result is a failure</exception>
        [Pure]
        public static TValue ValueOrThrow<TValue, TError>(this Result<TValue, TError> result)
        {
            return result.Case(
                success: value => value,
                failure: error => throw new ValueNotAvailableForFailure<TError>(error));
        }

        /// <summary>
        /// Returns the error when the result is a failure. When it is a success, throws.<para/>
        /// To be able to process both cases, use
        /// <see cref="Result{TValue,TError}.Case(Action{TValue},Action{TError})"/> or
        /// <see cref="Result{TValue,TError}.Case{T}(Func{TValue,T},Func{TError,T})"/>.
        /// <returns>The error when the result is a failure</returns>
        /// <exception cref="Exception">Thrown when the result is a success</exception>
        /// </summary>
        [Pure]
        public static TError ErrorOrThrow<TValue, TError>(this Result<TValue, TError> result)
        {
            return result.Case(
                success: value => throw new ErrorNotAvailableForSuccess<TValue>(value),
                failure: error => error);
        }

        private class ValueNotAvailableForFailure<TError> : Exception
        {
            public ValueNotAvailableForFailure(TError error)
                : base($"Result is a failure with error {error}")
            {
            }
        }
        
        private class ErrorNotAvailableForSuccess<TValue> : Exception
        {
            public ErrorNotAvailableForSuccess(TValue value)
                : base($"Result is a success with value {value}")
            {
            }
        }
    }
}