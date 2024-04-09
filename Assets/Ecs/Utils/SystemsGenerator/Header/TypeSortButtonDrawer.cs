using Ecs.Utils.SystemsGenerator.Sort.Impls;
using UnityEngine;

namespace Ecs.Utils.SystemsGenerator.Header
{
	public class TypeSortButtonDrawer : ASortButtonDrawer
	{
		private readonly TypeSortStrategy _sortStrategy;

		public TypeSortButtonDrawer(TypeSortStrategy sortStrategy, GUILayoutOption[] style)
			: base(sortStrategy, style)
		{
			_sortStrategy = sortStrategy;
		}

		protected override string GetButtonLabel() => _sortStrategy.Name.IsNullOrEmpty()
			? "Type"
			: _sortStrategy.Name;
	}
}