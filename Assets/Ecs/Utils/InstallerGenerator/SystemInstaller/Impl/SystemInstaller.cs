using System;
using Zenject;

namespace Ecs.Utils.InstallerGenerator.SystemInstaller.Impl
{
    public class SystemInstaller<T> : ISystemInstaller<T>
    {
        [Inject] private T _system;

        public string Name => typeof(T).FullName;
        
        object ISystemInstaller.System => _system;
        public T System => _system;

        public Enum Type { get; }
        public Enum Priority { get; }
        public int Order { get; }
        public Enum[] Features { get; }

        public SystemInstaller(
            Enum type,
            Enum priority,
            int order,
            params Enum[] features
        )
        {
            Type = type;
            Priority = priority;
            Order = order;
            Features = features;
        }
    }
}