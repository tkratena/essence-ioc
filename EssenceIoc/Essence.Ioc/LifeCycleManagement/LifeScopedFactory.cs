using System;

namespace Essence.Ioc.LifeCycleManagement
{
    internal static class LifeScopedFactory
    {
        public static T ConstructWithTracking<T>(this Func<T> factory, ILifeScope lifeScope)
        {
            return ConstructWithTracking(_ => factory.Invoke(), lifeScope);
        }

        public static T ConstructWithTracking<T>(this Func<ILifeScope, T> factory, ILifeScope lifeScope)
        {
            var instance = factory.Invoke(lifeScope);
            if (instance is IDisposable disposable)
            {
                lifeScope.TrackDisposable(disposable);
            }

            return instance;
        }
    }
}