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
    public class SystemBehaviour : ITickable, ILateTickable, IFixedTickable
    {
        private readonly ISystemInstaller<ISystem>[] _systems;
        private readonly IFeatureManager _featureManager;

        private readonly List<ISystemInstaller<ISystem>> _initializableSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _updateSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _fixedSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _lateSystems = new();
        private readonly List<ISystemInstaller<ISystem>> _reactiveSystems = new();
        private readonly HashSet<ISystem> _deactivatedReactiveSystems = new();
        
        public SystemBehaviour(
            IFeatureManager featureManager,
            ISystemInstaller[] systems
        )
        {
            _featureManager = featureManager;

            _systems = ProcessRawSystems(systems);
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
        
        public void LateTick()
        {
            Profiler.BeginSample("Late update systems");

            foreach (var systemInstaller in _lateSystems)
            {
                if (_featureManager.AnyEnable(systemInstaller.Features))
                {
                    Profiler.BeginSample(systemInstaller.Name);

                    (systemInstaller.System as ILateSystem)?.Late();

                    Profiler.EndSample();
                }
            }

            Profiler.EndSample();
        }

        public void FixedTick()
        {
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
            _lateSystems.Clear();
            _fixedSystems.Clear();
            _reactiveSystems.Clear();

            foreach (var systemInstaller in sortedSystems)
            {
                if (systemInstaller.System is IInitializeSystem)
                    _initializableSystems.Add(systemInstaller);
                if (systemInstaller.System is IUpdateSystem)
                    _updateSystems.Add(systemInstaller);
                if (systemInstaller.System is ILateSystem)
                    _lateSystems.Add(systemInstaller);
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