using System;
using System.Collections.Generic;

namespace Ecs.Core.Feature.Impl
{
    public class FeatureManager : IFeatureManager
    {
        private readonly Dictionary<Type, HashSet<Enum>> _disabledFeatures = new();

        public void SetFeatureActive(Enum feature, bool active)
        {
            var featureType = feature.GetType();
            if (active)
            {
                if (_disabledFeatures.TryGetValue(featureType, out var features))
                    features.Remove(feature);
            }
            else
            {
                if (!_disabledFeatures.ContainsKey(featureType))
                    _disabledFeatures[featureType] = new();

                var features = _disabledFeatures[featureType];
                features.Add(feature);
            }
        }

        public bool AnyEnable(params Enum[] features)
        {
            if (features.Length == 0)
                return true;

            foreach (var feature in features)
            {
                var type = feature.GetType();
                if (_disabledFeatures.TryGetValue(type, out var disabledFeatures))
                {
                    if (!disabledFeatures.Contains(feature))
                        return true;
                }
                else
                    return true;
            }

            return false;
        }

        public bool AnyDisable(params Enum[] features)
        {
            if (features.Length == 0)
                return false;

            foreach (var feature in features)
            {
                var type = feature.GetType();
                if (!_disabledFeatures.TryGetValue(type, out var disabledFeatures))
                    continue;

                if (disabledFeatures.Contains(feature))
                    return true;
            }

            return false;
        }
    }
}