using System.Collections.Generic;
using System.Linq;

namespace Ecs.Utils.SystemsGenerator.Sort.Impls
{
	public abstract class AAscendingOrDescendingSortStrategy<T> : ISortStrategy
	{
		public virtual string Name
		{
			get
			{
				switch (SortOrder)
				{
					case -1:
						return "ASC";
					case 0:
						return string.Empty;
					case 1:
						return "DESC";
					default:
						throw new System.Exception($"[{GetType().Name}] Wrong sort order");
				}
			}
		}

		protected int SortOrder;

		public void Reset() => SortOrder = 0;

		public void Next() => SortOrder = SortOrder == 0 ? 1 : SortOrder == 1 ? -1 : 0;

		public virtual void Sort(List<AttributeRecord> records)
		{
			if (SortOrder == 0)
				return;

			var attributeRecords = records.ToArray();
			var ordered = SortOrder == 1
				? attributeRecords.OrderByDescending(GetValue)
				: attributeRecords.OrderBy(GetValue);
			records.Clear();
			records.AddRange(ordered);
		}

		protected abstract T GetValue(AttributeRecord record);
	}
}