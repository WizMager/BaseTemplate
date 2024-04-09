using System;
using System.Collections.Generic;

namespace Ecs.Utils.SystemsGenerator.Sort.Impls
{
	public class TypeSortStrategy : ISortStrategy
	{
		private static readonly ExecutionType[] Types = (ExecutionType[])Enum.GetValues(typeof(ExecutionType));

		private int _filterType;

		public string Name => !IsFilterEmpty() ? Types[_filterType - 1].ToString() : string.Empty;

		public void Reset() => _filterType = 0;

		public void Next() => _filterType++;

		public void Sort(List<AttributeRecord> records)
		{
			if (IsFilterEmpty())
				return;

			var filterPriority = Types[_filterType - 1];
			records.Sort((record1, record2) =>
			{
				var type1 = record1.Attribute.Type;
				var type2 = record2.Attribute.Type;

				if (type1 == filterPriority && type2 != filterPriority)
					return 1;
				if (type1 != filterPriority && type2 == filterPriority)
					return -1;
				return 0;
			});
		}

		private bool IsFilterEmpty() => _filterType == 0;
	}
}