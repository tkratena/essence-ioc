using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Essence.Framework.Model
{
    public static class Result
    {
        [Pure]
        public static Success<TValue> Success<TValue>(TValue value)
        {
            return new Success<TValue>(value);
        }

        [Pure]
        public static Failure<TError> Failure<TError>(TError error)
        {
            return new Failure<TError>(error);
        }
    }

    /// <summary>
    /// Structure to be used as a return type. The result can be a success and contain a value or it can be a failure
    /// and contain an error.<para/>
    /// To use it, just define <see cref="Result{TValue,TError}"/> as return type of a method. Then, values and errors
    /// can be directly returned. Whether the result is a success or a failure will be inferred thanks to implicit
    /// conversion.<para/>
    /// If you want to be specific or if your value and error types are the same, you can use methods of
    /// <see cref="Result"/> to create the result; <see cref="Result.Success{TValue}"/> or
    /// <see cref="Result.Failure{TError}"/>.<para/>
    /// To consume the result, use
    /// <see cref="Result{TValue,TError}.Case(Action{TValue},Action{TError})"/> or
    /// <see cref="Result{TValue,TError}.Case{TResult}(Func{TValue,TResult},Func{TError,TResult})"/>.
    /// </summary>
    public struct Result<TValue, TError>
        : IEquatable<Result<TValue, TError>>, IEquatable<Success<TValue>>, IEquatable<Failure<TError>>
    {
        private readonly object _result;
        private readonly bool _isSuccess;

        public static implicit operator Result<TValue, TError>(TValue value)
        {
            return Success(value);
        }

        public static implicit operator Result<TValue, TError>(TError error)
        {
            return Failure(error);
        }

        public static implicit operator Result<TValue, TError>(Success<TValue> success)
        {
            return Success(success.Value);
        }

        public static implicit operator Result<TValue, TError>(Failure<TError> failure)
        {
            return Failure(failure.Error);
        }

        private static Result<TValue, TError> Success(TValue value)
        {
            return new Result<TValue, TError>(value, isSuccess: true);
        }

        private static Result<TValue, TError> Failure(TError error)
        {
            return new Result<TValue, TError>(error, isSuccess: false);
        }

        private Result(object result, bool isSuccess)
        {
            _result = result;
            _isSuccess = isSuccess;
        }
        
        private TValue Value => (TValue)_result;
        private TError Error => (TError)_result;

        public void Case(Action<TValue> success, Action<TError> failure)
        {
            if (_isSuccess)
            {
                success(Value);
            }
            else
            {
                failure(Error);
            }
        }

        [Pure]
        public TResult Case<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
        {
            return _isSuccess ? success(Value) : failure(Error);
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Success<TValue> other:
                    return Equals(other);
                
                case Failure<TError> other:
                    return Equals(other);
                
                default:
                    return base.Equals(obj);
            }
        }
        
        public bool Equals(Result<TValue, TError> other)
        {
            return Equals((object) other);
        }

        public bool Equals(Success<TValue> other)
        {
            return _isSuccess && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        public bool Equals(Failure<TError> other)
        {
            return !_isSuccess && EqualityComparer<TError>.Default.Equals(Error, other.Error);
        }

        [Pure]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _isSuccess 
                    ? EqualityComparer<TValue>.Default.GetHashCode(Value) 
                    : EqualityComparer<TError>.Default.GetHashCode(Error);
                hashCode = (hashCode * 397) ^ _isSuccess.GetHashCode();
                return hashCode;
            }
        }

        [Pure]
        public override string ToString()
        {
            return _isSuccess ? $"Success:{Value}" : $"Failure:{Error}";
        }
    }
}