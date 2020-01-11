using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Essence.Ioc.ExtendableRegistration;

namespace Essence.Ioc.FluentRegistration
{
    public interface IService<T>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T;

        [Pure]
        IService<T, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2;

        [Pure]
        IService<T1, T2, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3;

        [Pure]
        IService<T1, T2, T3, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4;

        [Pure]
        IService<T1, T2, T3, T4, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5;

        [Pure]
        IService<T1, T2, T3, T4, T5, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6, T7>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, T7, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, T7, T8, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, TService> AndService<TService>() where TService : class;
    }

    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    public interface IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        ILifeStyle ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10;

        ILifeStyle ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10;
    }
}