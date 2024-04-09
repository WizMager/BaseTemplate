using System;
using Zenject;

namespace Ecs.Core.Bootstrap
{
    public interface IBootstrap : IInitializable, IDisposable
    {
        void Pause(bool isPaused);
        void Reset();
    }
}