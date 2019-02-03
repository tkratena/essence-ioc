using System;
using Essence.Ioc.ExtendableRegistration;

namespace Essence.Ioc.FluentRegistration
{
    internal class Service<T1> : ServiceBase, IService<T1>
    {
        public Service(Registrations registrations) 
            : base(registrations, new[] {typeof(T1)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>() 
            where TServiceImplementation : class, T1
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1
        {
            return AddFactory(factory);
        }

        public IService<T1, T> AndService<T>() where T : class
        {
            return new Service<T1, T>(Registrations);
        }
    }

    internal class Service<T1, T2> : ServiceBase, IService<T1, T2>
    {
        public Service(Registrations registrations) 
            : base(registrations, new[] {typeof(T1), typeof(T2)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3> : ServiceBase, IService<T1, T2, T3>
    {
        public Service(Registrations registrations) 
            : base(registrations, new[] {typeof(T1), typeof(T2), typeof(T3)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>() 
            where TServiceImplementation : class, T1, T2, T3
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T>(Registrations);
        }
    }
    
    internal class Service<T1, T2, T3, T4> : ServiceBase, IService<T1, T2, T3, T4>
    {
        public Service(Registrations registrations) 
            : base(registrations, new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>() 
            where TServiceImplementation : class, T1, T2, T3, T4
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T4, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T4, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3, T4, T5> : ServiceBase, IService<T1, T2, T3, T4, T5>
    {
        public Service(Registrations registrations) 
            : base(registrations, new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>() 
            where TServiceImplementation : class, T1, T2, T3, T4, T5
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory) 
            where TServiceImplementation : class, T1, T2, T3, T4, T5
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T4, T5, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T4, T5, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3, T4, T5, T6> : ServiceBase, IService<T1, T2, T3, T4, T5, T6>
    {
        public Service(Registrations registrations) 
            : base(registrations, new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T4, T5, T6, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T4, T5, T6, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3, T4, T5, T6, T7> : ServiceBase, IService<T1, T2, T3, T4, T5, T6, T7>
    {
        public Service(Registrations registrations) : base(
            registrations, 
            new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T4, T5, T6, T7, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T4, T5, T6, T7, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3, T4, T5, T6, T7, T8> : ServiceBase, IService<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public Service(Registrations registrations) : base(
            registrations, 
            new[] {typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)})
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T4, T5, T6, T7, T8, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T4, T5, T6, T7, T8, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3, T4, T5, T6, T7, T8, T9> :
        ServiceBase, 
        IService<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        public Service(Registrations registrations) : base(
            registrations, 
            new[]
            {
                typeof(T1), 
                typeof(T2), 
                typeof(T3),
                typeof(T4), 
                typeof(T5),
                typeof(T6), 
                typeof(T7), 
                typeof(T8), 
                typeof(T9)
            })
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>() 
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory)
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9
        {
            return AddFactory(factory);
        }

        public IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> AndService<T>() where T : class
        {
            return new Service<T1, T2, T3, T4, T5, T6, T7, T8, T9, T>(Registrations);
        }
    }

    internal class Service<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : 
        ServiceBase,
        IService<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        public Service(Registrations registrations) : base(
            registrations, 
            new[]
            {
                typeof(T1), 
                typeof(T2),
                typeof(T3), 
                typeof(T4), 
                typeof(T5), 
                typeof(T6),
                typeof(T7), 
                typeof(T8), 
                typeof(T9), 
                typeof(T10)
            })
        {
        }

        public ILifeScope ImplementedBy<TServiceImplementation>()
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        {
            return AddImplementation<TServiceImplementation>();
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<TServiceImplementation> factory) 
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        {
            return AddFactory(factory);
        }

        public ILifeScope ConstructedBy<TServiceImplementation>(Func<IContainer, TServiceImplementation> factory) 
            where TServiceImplementation : class, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10
        {
            return AddFactory(factory);
        }           
    }
}