using System;
using System.Collections.Generic;
using System.Linq;

namespace Essence.Ioc.LifeCycleManagement
{
    internal class LifeScope : ILifeScope, IDisposable
    {
        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();
        private bool _isDisposed;

        public ILifeScope CreateNestedScope()
        {
            var nestedLifeScope = new LifeScope();
            TrackDisposable(nestedLifeScope);
            return nestedLifeScope;
        }

        public void TrackDisposable(IDisposable instance)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(LifeScope));
            }
            
            _disposables.Add(instance);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables.Reverse().Distinct())
            {
                disposable.Dispose();
            }
            
            _disposables.Clear();
            _isDisposed = true;
        }
    }
}