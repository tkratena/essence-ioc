using System;

namespace Essence.Ioc.LifeCycleManagement
{
    internal interface ILifeScope
    {
        void TrackDisposable(IDisposable instance);
    }
}