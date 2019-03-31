using System;
using System.Diagnostics.Contracts;

namespace Essence.Framework
{
    /// <summary>
    /// Structure to be used as a return type. The result can be a success and contain a value or it can be a failure
    /// and contain an error.<para/>
    /// To use it, just define <see cref="Result{TValue,TError}"/> as return type of a method. Then, values and errors
    /// can be directly returned. Whether the result is a success or a failure will be inferred thanks to implicit
    /// conversion.<para/>
    /// To consume the result, use
    /// <see cref="Result{TValue,TError}.Case(Action{TValue},Action{TError})"/> or
    /// <see cref="Result{TValue,TError}.Case{T}(Func{TValue,T},Func{TError,T})"/>.
    /// </summary>
    public struct Result<TValue, TError>
    {
        private readonly TValue _value;
        private readonly TError _error;
        private readonly bool _isSuccess;

        public static implicit operator Result<TValue, TError>(TValue value)
        {
            return Success(value);
        }

        public static implicit operator Result<TValue, TError>(TError error)
        {
            return Failure(error);
        }
        
        public static implicit operator Result<TValue, TError>(Result<TValue>.Success success)
        {
            return Success(success.Value);
        }
        
        public static implicit operator Result<TValue, TError>(Result<TError>.Failure failure)
        {
            return Failure(failure.Error);
        }

        private static Result<TValue, TError> Success(TValue value)
        {
            return new Result<TValue, TError>(value, default(TError), isSuccess: true);
        }

        private static Result<TValue, TError> Failure(TError error)
        {
            return new Result<TValue, TError>(default(TValue), error, isSuccess: false);
        }

        private Result(TValue value, TError error, bool isSuccess)
        {
            _value = value;
            _error = error;
            _isSuccess = isSuccess;
        }

        public void Case(Action<TValue> success, Action<TError> failure)
        {
            if (_isSuccess)
            {
                success(_value);
            }
            else
            {
                failure(_error);
            }
        }

        [Pure]
        public TResult Case<TResult>(Func<TValue, TResult> success, Func<TError, TResult> failure)
        {
            return _isSuccess ? success(_value) : failure(_error);
        }

        [Pure]
        public override string ToString()
        {
            return _isSuccess ? $"Success:{_value}" : $"Failure:{_error}";
        }
    }

    public static class Result
    {
        [Pure]
        public static Result<T>.Success Success<T>(T value)
        {
            return new Result<T>.Success(value);
        }

        [Pure]
        public static Result<T>.Failure Failure<T>(T error)
        {
            return new Result<T>.Failure(error);
        }
    }

    /// <summary>
    /// Structure to be used as a return type. The result can be a success and contain a value or it can be a failure
    /// and contain an error.<para/>
    /// To use it, just define <see cref="Result{T}"/> as return type of a method. Then, use methods of
    /// <see cref="Result"/> class to create the result; <see cref="Result.Success{T}"/> or
    /// <see cref="Result.Failure{T}"/>. These will be implicitly converted to <see cref="Result{T}"/>.<para/>
    /// To consume the result, use
    /// <see cref="Result{T}.Case(Action{T},Action{T})"/> or
    /// <see cref="Result{T}.Case{TResult}(Func{T,TResult},Func{T,TResult})"/>.<para/>
    /// </summary>
    public struct Result<T>
    {
        private readonly T _value;
        private readonly bool _isSuccess;

        public static implicit operator Result<T, T>(Result<T> result)
        {
            return result.Case(
                success: value => (Result<T, T>)Result.Success(value), 
                failure: error => (Result<T, T>)Result.Failure(error));
        }
        
        private Result(T value, bool isSuccess)
        {
            _value = value;
            _isSuccess = isSuccess;
        }

        public void Case(Action<T> success, Action<T> failure)
        {
            if (_isSuccess)
            {
                success(_value);
            }
            else
            {
                failure(_value);
            }
        }

        [Pure]
        public TResult Case<TResult>(Func<T, TResult> success, Func<T, TResult> failure)
        {
            return _isSuccess ? success(_value) : failure(_value);
        }
        
        [Pure]
        public override string ToString()
        {
            return _isSuccess ? $"Success:{_value}" : $"Failure:{_value}";
        }
        
        public struct Success
        {
            public T Value { get; }

            public Success(T value)
            {
                Value = value;
            }
            
            public static implicit operator Result<T>(Success success)
            {
                return new Result<T>(success.Value, isSuccess: true);
            }
        }
        
        public struct Failure
        {
            public T Error { get; }

            public Failure(T error)
            {
                Error = error;
            }
            
            public static implicit operator Result<T>(Failure failure)
            {
                return new Result<T>(failure.Error, isSuccess: false);
            }
        }
    }
}