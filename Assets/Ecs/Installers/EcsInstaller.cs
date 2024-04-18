using Ecs.Core;
using Ecs.Core.Feature.Impl;
using Ecs.Installers.Generated;
using Ecs.Utils.Groups.Impl;
using UnityEngine.Rendering;
using Zenject;

namespace Ecs.Installers
{
    public class EcsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FeatureManager>().AsSingle();
            
            Container.BindInterfacesTo<CommandBuffer>().AsSingle();
            
            BindGroups();
            
            BindContexts();
            
            InstallSystems();
            
            BindEventSystems();
            
            BindCleanupSystems();
            
            Container.BindInterfacesAndSelfTo<SystemBehaviour>().AsSingle().NonLazy();
        }
        
        private void BindGroups()
        {
            Container.BindInterfacesTo<GameGroupUtils>().AsSingle();
        }
        

        private void BindContexts()
        {
            foreach (var context in Contexts.SharedInstance.AllContexts)
            {
                Container.BindInterfacesAndSelfTo(context.GetType()).FromInstance(context).AsSingle();
            }

            Container.BindInterfacesAndSelfTo<Contexts>().FromInstance(Contexts.SharedInstance).AsSingle();
        }

        private void InstallSystems()
        {
            GameEcsSystems.Install(Container);
        }
        
        private void BindEventSystems()
        {
            Container.BindInterfacesTo<GameEventSystems>().AsSingle();
        }
        
        private void BindCleanupSystems()
        {
            Container.BindInterfacesTo<GameCleanupSystems>().AsSingle();
        }
    }
}