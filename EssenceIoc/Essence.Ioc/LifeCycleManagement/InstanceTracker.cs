using System;
using System.Collections.Generic;
using System.Linq;

namespace Essence.Ioc.LifeCycleManagement
{
    internal class InstanceTracker
    {
        private readonly ICollection<WeakReference<IDisposable>> _disposables = new List<WeakReference<IDisposable>>();
            
        public void TrackDisposable(IDisposable instance)
        {
            _disposables.Add(new WeakReference<IDisposable>(instance));
        }

        public void DisposeTrackedDisposables()
        {
            foreach (var disposable in GetTargets(_disposables).Reverse().Distinct())
            {
                disposable.Dispose();
            }
            
            _disposables.Clear();
        }

        private static IEnumerable<T> GetTargets<T>(IEnumerable<WeakReference<T>> references) where T : class
        {
            foreach (var weakReference in references)
            {
                if (weakReference.TryGetTarget(out var target))
                {
                    yield return target;
                }
            }
        }
    }
}