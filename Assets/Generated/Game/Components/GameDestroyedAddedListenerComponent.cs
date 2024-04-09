//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity
{
	public DestroyedAddedListenerComponent DestroyedAddedListener { get { return (DestroyedAddedListenerComponent)GetComponent(GameComponentsLookup.DestroyedAddedListener); } }
	public bool HasDestroyedAddedListener { get { return HasComponent(GameComponentsLookup.DestroyedAddedListener); } }

	public void AddDestroyedAddedListener(System.Collections.Generic.List<IDestroyedAddedListener> newValue)
	{
		var index = GameComponentsLookup.DestroyedAddedListener;
		var component = (DestroyedAddedListenerComponent)CreateComponent(index, typeof(DestroyedAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		AddComponent(index, component);
	}

	public void ReplaceDestroyedAddedListener(System.Collections.Generic.List<IDestroyedAddedListener> newValue)
	{
		var index = GameComponentsLookup.DestroyedAddedListener;
		var component = (DestroyedAddedListenerComponent)CreateComponent(index, typeof(DestroyedAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = newValue;
		#endif
		ReplaceComponent(index, component);
	}

	public void CopyDestroyedAddedListenerTo(DestroyedAddedListenerComponent copyComponent)
	{
		var index = GameComponentsLookup.DestroyedAddedListener;
		var component = (DestroyedAddedListenerComponent)CreateComponent(index, typeof(DestroyedAddedListenerComponent));
		#if !ENTITAS_REDUX_NO_IMPL
		component.value = copyComponent.value;
		#endif
		ReplaceComponent(index, component);
	}

	public void RemoveDestroyedAddedListener()
	{
		RemoveComponent(GameComponentsLookup.DestroyedAddedListener);
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher
{
	static JCMG.EntitasRedux.IMatcher<GameEntity> _matcherDestroyedAddedListener;

	public static JCMG.EntitasRedux.IMatcher<GameEntity> DestroyedAddedListener
	{
		get
		{
			if (_matcherDestroyedAddedListener == null)
			{
				var matcher = (JCMG.EntitasRedux.Matcher<GameEntity>)JCMG.EntitasRedux.Matcher<GameEntity>.AllOf(GameComponentsLookup.DestroyedAddedListener);
				matcher.ComponentNames = GameComponentsLookup.ComponentNames;
				_matcherDestroyedAddedListener = matcher;
			}

			return _matcherDestroyedAddedListener;
		}
	}
}

//------------------------------------------------------------------------------
// <auto-generated>
//		This code was generated by a tool (Genesis v2.4.7.0).
//
//
//		Changes to this file may cause incorrect behavior and will be lost if
//		the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity
{
	public void AddDestroyedAddedListener(IDestroyedAddedListener value)
	{
		var listeners = HasDestroyedAddedListener
			? DestroyedAddedListener.value
			: new System.Collections.Generic.List<IDestroyedAddedListener>();
		listeners.Add(value);
		ReplaceDestroyedAddedListener(listeners);
	}

	public void RemoveDestroyedAddedListener(IDestroyedAddedListener value, bool removeComponentWhenEmpty = true)
	{
		var listeners = DestroyedAddedListener.value;
		listeners.Remove(value);
		if (removeComponentWhenEmpty && listeners.Count == 0)
		{
			RemoveDestroyedAddedListener();
		}
		else
		{
			ReplaceDestroyedAddedListener(listeners);
		}
	}
}
