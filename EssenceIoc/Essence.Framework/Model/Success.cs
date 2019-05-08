using System;
using System.Collections.Generic;

namespace Essence.Framework.Model
{
    public struct Success<TValue> : IEquatable<Success<TValue>>
    {
        public TValue Value { get; }

        internal Success(TValue value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Success<TValue> other: 
                    return Equals(other);
                
                case IEquatable<Success<TValue>> other: 
                    return other.Equals(this);
                
                default:
                    return false;
            }
        }

        public bool Equals(Success<TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return ((Result<TValue, object>) this).GetHashCode();
        }
    }
}