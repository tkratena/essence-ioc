using System;
using System.Collections.Generic;

namespace Essence.Framework.Model
{
    public struct Failure<TError> : IEquatable<Failure<TError>>
    {
        public TError Error { get; }

        internal Failure(TError error)
        {
            Error = error;
        }
        
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Failure<TError> other: 
                    return Equals(other);
                
                case IEquatable<Failure<TError>> other: 
                    return other.Equals(this);
                
                default:
                    return false;
            }
        }

        public bool Equals(Failure<TError> other)
        {
            return EqualityComparer<TError>.Default.Equals(Error, other.Error);
        }

        public override int GetHashCode()
        {
            return ((Result<object, TError>) this).GetHashCode();
        }
    }
}