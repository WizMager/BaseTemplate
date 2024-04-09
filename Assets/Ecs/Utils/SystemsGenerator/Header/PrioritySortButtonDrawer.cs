using Ecs.Utils.SystemsGenerator.Sort.Impls;
using UnityEngine;

namespace Ecs.Utils.SystemsGenerator.Header
{
	public class PrioritySortButtonDrawer : ASortButtonDrawer
	{
		private readonly PrioritySortStrategy _sortStrategy;

		public PrioritySortButtonDrawer(PrioritySortStrategy sortStrategy, GUILayoutOption[] style)
			: base(sortStrategy, style)
		{
			_sortStrategy = sortStrategy;
		}

		protected override string GetButtonLabel() => _sortStrategy.Name.IsNullOrEmpty()
			? "Priority"
			: _sortStrategy.Name;
	}
}