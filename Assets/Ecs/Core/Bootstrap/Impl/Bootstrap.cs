using System;
using System.Collections.Generic;
using Ecs.Core.Interfaces;
using JCMG.EntitasRedux;
using Zenject;

namespace Ecs.Core.Bootstrap.Impl
{
    public class Bootstrap : IBootstrap, ITickable, ILateTickable, IFixedTickable, IGuiRenderable
    {
        private readonly Contexts _contexts;
        private readonly CustomFeature _feature;
        private readonly List<IStartable> _startables;
        private readonly List<IResetable> _resetables;
        private readonly List<ILateSystem> _late = new();
        private readonly List<IFixedSystem> _fixed = new();
        private readonly List<IGuiSystem> _gui = new();
        private bool _isInitialized;
        private bool _isPaused;
        
        public Bootstrap(
            [InjectLocal] CustomFeature feature,
            [InjectLocal] Contexts contexts,
            [InjectLocal] List<ISystem> systems,
            [InjectLocal] List<IStartable> startables,
            [InjectLocal] List<IResetable> resetables
        )
        {
            _contexts = contexts;
            _startables = startables;
            _resetables = resetables;
            _feature = feature;
            foreach (var system in systems)
            {
                _feature.Add(system);
                
                if (system is ILateSystem late)
                    _late.Add(late);
                
                if (system is IFixedSystem fixedSystem)
                    _fixed.Add(fixedSystem);
                
                if (system is IGuiSystem gui)
                    _gui.Add(gui);
            }
        }

        public void Initialize()
        {
            if (_isInitialized)
                throw new Exception($"[{typeof(Bootstrap)}]: Bootstrap already is initialized");

            if (_startables != null)
                foreach (var pool in _startables)
                    pool.Start();
            
            _feature.Initialize();
            _isInitialized = true;
        }

        public void Tick()
        {
            if (_isPaused)
                return;

            _feature.Update();
        }

        public void LateTick()
        {
            if (_isPaused)
                return;

            foreach (var lateSystem in _late)
                lateSystem.Late();

            _feature.Cleanup();
        }

        public void FixedTick()
        {
            if (_isPaused)
                return;

            foreach (var fixedSystem in _fixed)
                fixedSystem.Fixed();
        }

        public void GuiRender()
        {
            if (_isPaused)
                return;

            foreach (var guiSystem in _gui)
                guiSystem.Gui();
        }
        
        public void Pause(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void Reset()
        {
            Pause(true);

            _feature.Deactivate();
            foreach (var context in _contexts.AllContexts)
            {
                context.DestroyAllEntities();
                context.ResetCreationIndex();
            }

            foreach (var resetable in _resetables)
                resetable.Reset();

            _feature.Activate();
            _isInitialized = false;
            Initialize();

            Pause(false);
        }
        
        public void Dispose()
        {
            _feature.Deactivate();
            _contexts.Reset();
        }
    }
}