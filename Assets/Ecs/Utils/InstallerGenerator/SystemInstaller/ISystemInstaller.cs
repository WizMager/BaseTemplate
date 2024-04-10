using System;

namespace Ecs.Utils.InstallerGenerator.SystemInstaller
{
    public interface ISystemInstaller
    {
        string Name { get; }
        
        object System { get; }
        Enum Type { get; }
        Enum Priority { get; }
        int Order { get; }
        Enum[] Features { get; }
    }
    
    public interface ISystemInstaller<out T> : ISystemInstaller
    {
        T System { get; }
    }
}