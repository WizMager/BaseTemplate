using Ecs.Core.Feature;
using Ecs.Utils.Groups;
using JCMG.EntitasRedux;

namespace Ecs.Core.Systems
{
    [Install(EFeatureType.Game, EPriority.Low, int.MaxValue)]
    public class GameDestroySystem : IUpdateSystem
    {
        private readonly IGameGroupUtils _gameGroupUtils;
        
        public GameDestroySystem(IGameGroupUtils gameGroupUtils)
        {
            _gameGroupUtils = gameGroupUtils;
        }
        
        public void Update()
        {
            using var _ = _gameGroupUtils.GetDestroyed(out var destroyedEntities);
            
            foreach (var entity in destroyedEntities)
            {
                entity.Destroy();
            }
        }
    }
}