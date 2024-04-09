using JCMG.EntitasRedux;

namespace Ecs.Core.Interfaces
{
    public interface ILateSystem : ISystem
    {
        void Late();
    }
}