using JCMG.EntitasRedux;

namespace Ecs.Core.Components
{
    [Game]
    [Event(EventTarget.Self)]
    [Cleanup(CleanupMode.DestroyEntity)]
    public class DestroyedComponent : IComponent
    {
    }
}