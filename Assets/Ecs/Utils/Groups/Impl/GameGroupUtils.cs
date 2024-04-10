using System;
using System.Collections.Generic;
using JCMG.EntitasRedux;
using UnityEngine.Pool;
using Utils.Extensions;

namespace Ecs.Utils.Groups.Impl
{
    public class GameGroupUtils : IGameGroupUtils
    {
         //private readonly IGroup<GameEntity> _obstacleGroup;

        public GameGroupUtils(GameContext game)
        {
            //_obstacleGroup = game.GetGroup(GameMatcher.AllOf(GameMatcher.Obstacle));
        }

        // public IDisposable GetObstacles(out List<GameEntity> buffer, Func<GameEntity, bool> filter = null)
        // {
        //     return GetEntities(out buffer, _obstacleGroup, e => e.HasObstacle && !e.IsDestroyed, filter);
        // }
        
        private IDisposable GetEntities(
            out List<GameEntity> buffer,  
            IGroup<GameEntity> group,
            Func<GameEntity, bool> baseFilter, 
            Func<GameEntity, bool> filter = null)
        {
            var pooledObject = ListPool<GameEntity>.Get(out buffer);
            group.GetEntities(buffer);
            
            if (filter != null)
            {
                buffer.RemoveAllWithSwap(e => !(baseFilter(e) && filter(e)));    
            }
            else
            {
                buffer.RemoveAllWithSwap(e => !baseFilter(e));
            }
            
            return pooledObject;
        }
    }
}