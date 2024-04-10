using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Core.Feature;
using Ecs.Core.Interfaces;
using Ecs.Utils.InstallerGenerator.SystemInstaller;
using JCMG.EntitasRedux;
using UnityEngine.Profiling;
using Zenject;

namespace Ecs.Core
{
    public class Bootstrap : ITickable, ILateTickable, IFixedTickable, IDisposable
    {
        private readonly ISystemInstaller<ISystem>[] _systems;
        private readonly IFeatureManager _featureManager;

        private readonly List<ISystemInstaller<ISystem>> _initializableSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _updateSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _fixedSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _reactiveSystems = new();
        private readonly HashSet<ISystem> _deactivatedReactiveSystems = new();
        
        private readonly Contexts _contexts;
        //private readonly CustomFeature _feature;
        // private readonly List<IStartable> _startables;
        // private readonly List<IResetable> _resetables;
        // private readonly List<ILateSystem> _late = new();
        private bool _isInitialized;
        private bool _isPaused;
        
        public Bootstrap(
            IFeatureManager featureManager,
            ISystemInstaller[] systems
        )
        {
            _featureManager = featureManager;

            _systems = ProcessRawSystems(systems);
        }

        // public void Initialize()
        // {
        //     if (_isInitialized)
        //         throw new Exception($"[{typeof(Bootstrap)}]: Bootstrap already is initialized");
        //
        //     if (_startables != null)
        //         foreach (var pool in _startables)
        //             pool.Start();
        //     
        //     _feature.Initialize();
        //     _isInitialized = true;
        // }

        // public void Tick()
        // {
        //     if (_isPaused)
        //         return;
        //
        //     _feature.Update();
        // }

        public void LateTick()
        {
            if (_isPaused)
                return;
        
            // foreach (var lateSystem in _late)
            //     lateSystem.Late();
        
            //_feature.Cleanup();
        }
        
        public void Pause(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void Reset()
        {
            Pause(true);

            //_feature.Deactivate();
            foreach (var context in _contexts.AllContexts)
            {
                context.DestroyAllEntities();
                context.ResetCreationIndex();
            }

            // foreach (var resetable in _resetables)
            //     resetable.Reset();

            //_feature.Activate();
            _isInitialized = false;
            Initialize();

            Pause(false);
        }
        
        public void Dispose()
        {
            //_feature.Deactivate();
            _contexts.Reset();
        }
        
        public void Initialize()
        {
            Profiler.BeginSample("Initialize systems");

            foreach (var systemInstaller in _initializableSystems)
            {
                if (_featureManager.AnyEnable(systemInstaller.Features))
                {
                    Profiler.BeginSample(systemInstaller.Name);

                    (systemInstaller.System as IInitializeSystem)?.Initialize();

                    Profiler.EndSample();
                }
            }

            Profiler.EndSample();
        }

        public void Tick()
        {
            UpdateReactiveSystemsState();

            Profiler.BeginSample("Update systems");

            foreach (var systemInstaller in _updateSystems)
            {
                if (_featureManager.AnyEnable(systemInstaller.Features))
                {
                    Profiler.BeginSample(systemInstaller.Name);

                    (systemInstaller.System as IUpdateSystem)?.Update();

                    Profiler.EndSample();
                }
            }

            Profiler.EndSample();
        }

        public void FixedTick()
        {
            if (_isPaused)
                return;
            
            Profiler.BeginSample("Fixed update systems");

            foreach (var systemInstaller in _fixedSystems)
            {
                if (_featureManager.AnyEnable(systemInstaller.Features))
                {
                    Profiler.BeginSample(systemInstaller.Name);

                    (systemInstaller.System as IFixedSystem)?.FixedExecute();

                    Profiler.EndSample();
                }
            }

            Profiler.EndSample();
        }

        private void UpdateReactiveSystemsState()
        {
            if (_reactiveSystems.Count == 0)
                return;

            Profiler.BeginSample("Update Reactive Systems State");

            foreach (var systemInstaller in _reactiveSystems)
            {
                if (_featureManager.AnyEnable(systemInstaller.Features))
                {
                    var system = systemInstaller.System;
                    if (!_deactivatedReactiveSystems.Contains(system))
                        continue;

                    var reactiveSystem = systemInstaller.System as IReactiveSystem;
                    reactiveSystem?.Activate();

                    _deactivatedReactiveSystems.Remove(system);
                }
                else
                {
                    var system = systemInstaller.System;
                    if (_deactivatedReactiveSystems.Contains(system))
                        continue;

                    var reactiveSystem = systemInstaller.System as IReactiveSystem;
                    reactiveSystem?.Deactivate();

                    _deactivatedReactiveSystems.Add(reactiveSystem);
                }
            }

            Profiler.EndSample();
        }
        
        private ISystemInstaller<ISystem>[] ProcessRawSystems(ISystemInstaller[] rawSystems)
        {
            Profiler.BeginSample("Install systems");

            var sortedSystems = rawSystems
                .Select(s => (ISystemInstaller<ISystem>)s)
                .Where(s => s != null)
                .OrderByDescending(s => s.Priority)
                .ThenBy(s => s.Order)
                .ToArray();

            _initializableSystems.Clear();
            _updateSystems.Clear();
            _fixedSystems.Clear();
            _reactiveSystems.Clear();

            foreach (var systemInstaller in sortedSystems)
            {
                if (systemInstaller.System is IInitializeSystem)
                    _initializableSystems.Add(systemInstaller);
                if (systemInstaller.System is IUpdateSystem)
                    _updateSystems.Add(systemInstaller);
                if (systemInstaller.System is IFixedSystem)
                    _fixedSystems.Add(systemInstaller);
                if (systemInstaller.System is IReactiveSystem)
                {
                    _reactiveSystems.Add(systemInstaller);
                    _deactivatedReactiveSystems.Add(systemInstaller.System);
                }
            }

            Profiler.EndSample();

            return sortedSystems;
        }
    }
}