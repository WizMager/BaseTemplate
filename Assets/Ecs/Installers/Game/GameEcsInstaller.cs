using Ecs.Installers.Game.Feature;
using Ecs.Installers.Game.Feature.Impl;
using Ecs.Utils.Groups.Impl;
using UnityEngine.Rendering;

namespace Ecs.Installers.Game
{
    public class GameEcsInstaller : AEcsInstaller
    {
        
        protected override void InstallSystems(Contexts contexts)
        {
            BindGroups();
            
            Container.BindInterfacesTo<CommandBuffer>().AsSingle();
			
            BindContext<GameContext>();
            
            //GameEcsSystems.Install(Container);
            
            // Event systems
            Container.BindInterfacesTo<GameEventSystems>().AsSingle();
            
            //Cleanup systems
            Container.BindInterfacesTo<GameCleanupSystems>().AsSingle();
            
            BindFeature<GameFeature, IGameFeature>();
        }
        
        private void BindGroups()
        {
            Container.BindInterfacesTo<GameGroupUtils>().AsSingle();
        }
    }
}