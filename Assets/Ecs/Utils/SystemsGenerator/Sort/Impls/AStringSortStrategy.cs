using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecs.Utils.SystemsGenerator.Sort.Impls
{
	public abstract class AStringSortStrategy : AAscendingOrDescendingSortStrategy<string>
	{
		protected static readonly StringComparer Comparer = new();
		
		public override string Name
		{
			get
			{
				switch (SortOrder)
				{
					case -1:
						return "z-a";
					case 0:
						return string.Empty;
					case 1:
						return "a-z";
					default:
						throw new System.Exception($"[{nameof(AStringSortStrategy)}] Wrong sort order");
				}
			}
		}

		public override void Sort(List<AttributeRecord> records)
		{
			if (SortOrder == 0)
				return;

			var attributeRecords = records.ToArray();
			var ordered = SortOrder == 1
				? attributeRecords.OrderBy(GetValue, Comparer)
				: attributeRecords.OrderByDescending(GetValue);
			records.Clear();
			records.AddRange(ordered);
		}

		protected class StringComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				var isEmptyX = x.IsNullOrEmpty();
				var isEmptyY = y.IsNullOrEmpty();

				if (isEmptyX && isEmptyY)
					return 0;
				if (isEmptyX)
					return 1;
				if (isEmptyY)
					return -1;
				return string.Compare(x, y, StringComparison.Ordinal);
			}
		}
	}
}