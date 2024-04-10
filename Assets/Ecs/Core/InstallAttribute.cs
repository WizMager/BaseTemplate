using System;
using System.Linq;
using Ecs.Core.Feature;
using Ecs.Utils.InstallerGenerator.Attributes;

namespace Ecs.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InstallAttribute : AInstallAttribute
    {
        public InstallAttribute(EFeatureType type, EPriority priority, int order, params EFeature[] features) : base(type, priority, order, features.Cast<Enum>().ToArray())
        {
        }
    }
}