using System.Collections.Generic;

namespace Ecs.Utils.SystemsGenerator.Sort.Impls
{
	public class PrioritySortStrategy : ISortStrategy
	{
		private const int Delimiter = (int) ExecutionPriority.None + 2;

		private int _filterPriority;

		public string Name => !IsFilterEmpty()
			? ((ExecutionPriority) (_filterPriority % Delimiter - 1)).ToString()
			: string.Empty;

		public void Reset() => _filterPriority = 0;

		public void Next() => _filterPriority++;

		public void Sort(List<AttributeRecord> records)
		{
			if (IsFilterEmpty())
				return;

			var filterPriority = (ExecutionPriority) (_filterPriority % Delimiter - 1);
			records.Sort((record1, record2) =>
			{
				var priority1 = record1.Attribute.Priority;
				var priority2 = record2.Attribute.Priority;

				if (priority1 == filterPriority && priority2 != filterPriority)
					return -1;
				if (priority1 != filterPriority && priority2 == filterPriority)
					return 1;
				return 0;
			});
		}

		private bool IsFilterEmpty() => _filterPriority % Delimiter == 0;
	}
}