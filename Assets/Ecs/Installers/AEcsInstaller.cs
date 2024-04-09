using System;
using Ecs.Core.Bootstrap.Impl;
using JCMG.EntitasRedux;
using Zenject;

namespace Ecs.Installers
{
    public abstract class AEcsInstaller : MonoInstaller, IDisposable
    {
        private Contexts _contexts;

        public override void InstallBindings()
        {
            Container.Bind<IDisposable>().FromInstance(this).AsTransient();

            _contexts = Contexts.SharedInstance;
            InstallSystems(_contexts);
            
            Container.BindInstance(_contexts).WhenInjectedInto<Core.Bootstrap.Impl.Bootstrap>();
            Container.BindInterfacesTo<Core.Bootstrap.Impl.Bootstrap>().AsSingle().NonLazy();
        }

        protected abstract void InstallSystems(Contexts contexts);

        protected void BindEventSystem<TEventSystem>()
            where TEventSystem : Feature
        {
            Container.BindInterfacesTo<TEventSystem>().AsSingle().WithArguments(_contexts);
        }

        protected void BindContext<TContext>()
            where TContext : IContext
        {
            foreach (var ctx in _contexts.AllContexts)
                if (ctx is TContext context)
                {
                    Container.BindInterfacesAndSelfTo<TContext>().FromInstance(context).AsSingle();
                    return;
                }

            throw new Exception($"[{nameof(AEcsInstaller)}] No context with type: {typeof(TContext).Name}");
        }

        protected void BindFeature<TConcrete, TContract>()
            where TConcrete : CustomFeature, TContract, new()
            where TContract : ICustomFeature
        {
            var mainFeature = new TConcrete();
            Container.Bind<TContract>().FromInstance(mainFeature);
            Container.Bind<CustomFeature>().FromInstance(mainFeature).WhenInjectedInto<Core.Bootstrap.Impl.Bootstrap>();
        }

        public void Dispose()
        {
        }
    }
}