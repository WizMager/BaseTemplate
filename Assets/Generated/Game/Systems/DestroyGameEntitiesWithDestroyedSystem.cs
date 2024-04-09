//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using JCMG.EntitasRedux;

public sealed class DestroyGameEntitiesWithDestroyedSystem : ICleanupSystem
{
	private readonly IGroup<GameEntity> _group;
	private readonly List<GameEntity> _entities;

	public DestroyGameEntitiesWithDestroyedSystem(IContext<GameEntity> context)
	{
		_group = context.GetGroup(GameMatcher.Destroyed);
		_entities = new List<GameEntity>();
	}

	/// <summary>
	/// Performs cleanup logic after other systems have executed.
	/// </summary>
	public void Cleanup()
	{
		_group.GetEntities(_entities);
		for (var i = 0; i < _entities.Count; ++i)
		{
			_entities[i].Destroy();
		}
	}
}
