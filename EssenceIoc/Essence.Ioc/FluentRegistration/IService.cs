using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Essence.Ioc.FluentRegistration
{
    public interface IService<T>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T;

        [Pure]
        IService<T, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2;

        [Pure]
        IService<T1, T2, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3;

        [Pure]
        IService<T1, T2, T3, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4;

        [Pure]
        IService<T1, T2, T3, T4, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5;

        [Pure]
        IService<T1, T2, T3, T4, T5, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6, T7>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, T7, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, T7, T8, TService> AndService<TService>() where TService : class;
    }

    public interface IService<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9;

        [Pure]
        IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, TService> AndService<TService>() where TService : class;
    }

    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")]
    public interface IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10;

        ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10;
    }
}