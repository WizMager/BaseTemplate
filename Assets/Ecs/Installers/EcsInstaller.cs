using System;
using Ecs.Core;
using Ecs.Core.Feature.Impl;
using Ecs.Utils.Groups.Impl;
using JCMG.EntitasRedux;
using UnityEngine.Rendering;
using Zenject;

namespace Ecs.Installers
{
    public class EcsInstaller : MonoInstaller, IDisposable
    {
        public override void InstallBindings()
        {
            Container.Bind<IDisposable>().FromInstance(this).AsTransient();
            
            Container.BindInterfacesAndSelfTo<FeatureManager>().AsSingle();
            
            Container.BindInterfacesTo<CommandBuffer>().AsSingle();
            
            BindGroups();
            
            BindContexts();
            
            InstallSystems();
            
            BindEventSystems();
            
            BindCleanupSystems();
            
            //Container.BindInstance(_contexts).WhenInjectedInto<Bootstrap>();
            //Container.BindInterfacesTo<Bootstrap>().AsSingle().NonLazy();
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
            //GameEcsSystems.Install(Container);
        }
        
        private void BindEventSystems()
        {
            Container.BindInterfacesTo<GameEventSystems>().AsSingle();
        }
        
        private void BindCleanupSystems()
        {
            Container.BindInterfacesTo<GameCleanupSystems>().AsSingle();
        }

        // protected void BindFeature<TConcrete, TContract>()
        //     where TConcrete : CustomFeature, TContract, new()
        //     where TContract : ICustomFeature
        // {
        //     var mainFeature = new TConcrete();
        //     Container.Bind<TContract>().FromInstance(mainFeature);
        //     Container.Bind<CustomFeature>().FromInstance(mainFeature).WhenInjectedInto<Bootstrap>();
        // }

        public void Dispose()
        {
        }
    }
}