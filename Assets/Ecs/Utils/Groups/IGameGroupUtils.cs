using System;
using System.Collections.Generic;

namespace Ecs.Utils.Groups
{
    public interface IGameGroupUtils
    {
        IDisposable GetDestroyed(out List<GameEntity> buffer, Func<GameEntity, bool> filter = null);
    }
}