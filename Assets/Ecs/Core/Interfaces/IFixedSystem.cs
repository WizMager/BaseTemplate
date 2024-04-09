using JCMG.EntitasRedux;

namespace Ecs.Core.Interfaces
{
    public interface IFixedSystem : ISystem
    {
        void Fixed();
    }
}