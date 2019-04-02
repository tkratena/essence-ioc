using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Essence.Framework.Model
{
    /// <summary>
    /// Structure to be used as a return type. The result can be a success or a failure.<para/>
    /// To use it, just define <see cref="Result"/> as return type of a method. Then to create the result, use
    /// <see cref="Result.Success"/> or <see cref="Result.Failure"/>. These will be implicitly converted to
    /// <see cref="Result"/>. Or you can make use of implicit conversion from <see cref="bool"/>.<para/>
    /// To consume the result, use
    /// <see cref="Result.Case(Action,Action)"/> or <see cref="Result.Case{TResult}(Func{TResult},Func{TResult})"/>.
    /// <para/>
    /// </summary>
    public struct Result
    {
        private readonly bool _isSuccess;
        
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
        
        [Pure]
        public static Result Success()
        {
            return new Result(isSuccess: true);
        }
        
        [Pure]
        public static Failure Failure()
        {
            return new Failure();
        }
        
        public static implicit operator Result(Failure failure)
        {
            return new Result(isSuccess: false);
        }
        
        public static implicit operator Result(bool isSuccess)
        {
            return new Result(isSuccess);
        }

        private Result(bool isSuccess)
        {
            _isSuccess = isSuccess;
        }

        public void Case(Action success, Action failure)
        {
            if (_isSuccess)
            {
                success();
            }
            else
            {
                failure();
            }
        }

        [Pure]
        public TResult Case<TResult>(Func<TResult> success, Func<TResult> failure)
        {
            return _isSuccess ? success() : failure();
        }

        [Pure]
        public override int GetHashCode()
        {
            return _isSuccess.GetHashCode();
        }

        [Pure]
        public override string ToString()
        {
            return _isSuccess ? "Success" : "Failure";
        }
    }

    /// <summary>
    /// Structure to be used as a return type. The result can be a success and contain a value or it can be a failure.
    /// <para/>
    /// To use it, just define <see cref="Result{TValue}"/> as return type of a method. Then, values can be directly
    /// returned as success thanks to implicit conversion. Use <see cref="Result.Failure"/> method of
    /// <see cref="Result"/> to create a failure.<para/>
    /// Then, use methods of
    /// If you want to be specific, you can use <see cref="Result.Success{TValue}"/> method of <see cref="Result"/>.
    /// <see cref="Result.Failure{TError}"/>.<para/>
    /// To consume the result, use
    /// <see cref="Result{TValue}.Case(Action{TValue},Action)"/> or
    /// <see cref="Result{TValue}.Case{TResult}(Func{TValue,TResult},Func{TResult})"/>.<para/>
    /// </summary>
    public struct Result<TValue>
    {
        private readonly TValue _value;
        private readonly bool _isSuccess;

        public static implicit operator Result<TValue>(TValue value)
        {
            return new Result<TValue>(value);
        }
        
        public static implicit operator Result<TValue>(Success<TValue> success)
        {
            return new Result<TValue>(success.Value);
        }
        
        public static implicit operator Result<TValue>(Failure failure)
        {
            return new Result<TValue>();
        }

        private Result(TValue value)
        {
            _value = value;
            _isSuccess = true;
        }

        public void Case(Action<TValue> success, Action failure)
        {
            if (_isSuccess)
            {
                success(_value);
            }
            else
            {
                failure();
            }
        }

        [Pure]
        public TResult Case<TResult>(Func<TValue, TResult> success, Func<TResult> failure)
        {
            return _isSuccess ? success(_value) : failure();
        }

        [Pure]
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TValue>.Default.GetHashCode(_value) * 397) ^ _isSuccess.GetHashCode();
            }
        }

        [Pure]
        public override string ToString()
        {
            return _isSuccess ? $"Success:{_value}" : "Failure";
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
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<TValue>.Default.GetHashCode(_value);
                hashCode = (hashCode * 397) ^ EqualityComparer<TError>.Default.GetHashCode(_error);
                hashCode = (hashCode * 397) ^ _isSuccess.GetHashCode();
                return hashCode;
            }
        }

        [Pure]
        public override string ToString()
        {
            return _isSuccess ? $"Success:{_value}" : $"Failure:{_error}";
        }
    }
}