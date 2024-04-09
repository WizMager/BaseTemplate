using System;
using JCMG.EntitasRedux;

namespace Ecs.Core.Bootstrap.Impl
{
	public interface ICustomFeature
		{
			void AddBefore<TSystem>(ISystem system) where TSystem : ISystem;
			void AddAfter<TSystem>(ISystem system) where TSystem : ISystem;
			void Remove(ISystem system);
		}

		public class CustomFeature : Systems, ICustomFeature
		{
			public void AddBefore<TSystem>(ISystem system) where TSystem : ISystem
			{
				if (system is IInitializeSystem initializeSystem)
				{
					var index = GetInitializeSystemIndex<TSystem>();
					_initializeSystems.Insert(index, initializeSystem);
				}

				if (system is IUpdateSystem executeSystem)
				{
					var index = GetExecuteSystemIndex<TSystem>();
					_updateSystems.Insert(index, executeSystem);
				}

				if (system is ICleanupSystem cleanupSystem)
				{
					var index = GetCleanupSystemIndex<TSystem>();
					_cleanupSystems.Insert(index, cleanupSystem);
				}

				if (system is ITearDownSystem tearDownSystem)
				{
					var index = GetTearDownSystemIndex<TSystem>();
					_tearDownSystems.Insert(index, tearDownSystem);
				}

				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Activate();
					_reactiveSystems.Add(reactiveSystem);
				}
			}

			public void AddAfter<TSystem>(ISystem system) where TSystem : ISystem
			{
				if (system is IInitializeSystem initializeSystem)
				{
					var index = GetInitializeSystemIndex<TSystem>();
					_initializeSystems.Insert(index + 1, initializeSystem);
				}

				if (system is IUpdateSystem executeSystem)
				{
					var index = GetExecuteSystemIndex<TSystem>();
					_updateSystems.Insert(index + 1, executeSystem);
				}

				if (system is ICleanupSystem cleanupSystem)
				{
					var index = GetCleanupSystemIndex<TSystem>();
					_cleanupSystems.Insert(index + 1, cleanupSystem);
				}

				if (system is ITearDownSystem tearDownSystem)
				{
					var index = GetTearDownSystemIndex<TSystem>();
					_tearDownSystems.Insert(index + 1, tearDownSystem);
				}

				if (system is IReactiveSystem reactiveSystem)
				{
					var index = GetTearDownSystemIndex<TSystem>();
					_reactiveSystems.Insert(index + 1, reactiveSystem);
				}
			}

			public void Remove(ISystem system)
			{
				if (system is IReactiveSystem reactiveSystem)
				{
					reactiveSystem.Deactivate();
					_reactiveSystems.Remove(reactiveSystem);
					return;
				}

				if (system is IInitializeSystem initializeSystem) _initializeSystems.Remove(initializeSystem);

				if (system is IUpdateSystem executeSystem) _updateSystems.Remove(executeSystem);

				if (system is ICleanupSystem cleanupSystem) _cleanupSystems.Remove(cleanupSystem);

				if (system is ITearDownSystem tearDownSystem) _tearDownSystems.Remove(tearDownSystem);
			}

			private int GetInitializeSystemIndex<TSystem>() where TSystem : ISystem
			{
				for (var i = 0; i < _initializeSystems.Count; i++)
				{
					var system = _initializeSystems[i];
					if (system.GetType() == typeof(TSystem))
						return i;
				}

				throw new ArgumentException("[MainFeature] no initialize system " + typeof(TSystem).Name);
			}

			private int GetCleanupSystemIndex<TSystem>() where TSystem : ISystem
			{
				for (var i = 0; i < _cleanupSystems.Count; i++)
				{
					var system = _cleanupSystems[i];
					if (system.GetType() == typeof(TSystem))
						return i;
				}

				throw new ArgumentException("[MainFeature] no cleanup system " + typeof(TSystem).Name);
			}

			private int GetExecuteSystemIndex<TSystem>() where TSystem : ISystem
			{
				for (var i = 0; i < _updateSystems.Count; i++)
				{
					var system = _updateSystems[i];
					if (system.GetType() == typeof(TSystem))
						return i;
				}

				throw new ArgumentException("[MainFeature] no execute system " + typeof(TSystem).Name);
			}

			private int GetTearDownSystemIndex<TSystem>() where TSystem : ISystem
			{
				for (var i = 0; i < _tearDownSystems.Count; i++)
				{
					var system = _tearDownSystems[i];
					if (system.GetType() == typeof(TSystem))
						return i;
				}

				throw new ArgumentException("[MainFeature] no tear down system " + typeof(TSystem).Name);
			}
		}
}