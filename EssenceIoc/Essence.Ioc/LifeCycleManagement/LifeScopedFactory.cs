using System;

namespace Essence.Ioc.LifeCycleManagement
{
    internal static class LifeScopedFactory
    {
        public static T ConstructWithTracking<T>(this Func<T> factory, ILifeScope lifeScope)
        {
            var instance = factory.Invoke();
            if (instance is IDisposable disposable)
            {
                lifeScope.TrackDisposable(disposable);
            }

            return instance;
        }
    }
}