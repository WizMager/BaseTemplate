using System;

namespace Ecs.Core.Feature
{
    public interface IFeatureManager
    {
        void SetFeatureActive(Enum feature, bool active);

        bool AnyEnable(params Enum[] features);
        bool AnyDisable(params Enum[] features);
    }
}