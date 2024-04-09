using System.Collections.Generic;
using System.Linq;

namespace Ecs.Utils.SystemsGenerator.Sort.Impls
{
	public class FeatureSortStrategy : ISortStrategy
	{
		public IFeatureFilter Filter;

		public string Name => string.Empty;

		public void Next()
		{
		}

		public void Reset() => Filter.Reset();

		public void Sort(List<AttributeRecord> records)
		{
			var featuresFilter = Filter.GetFeatures();
			var filteredRecords = records.Where(record => featuresFilter.Any(f => record.Features.Contains(f))).ToArray();
			var groupBy = filteredRecords.GroupBy(record => record.Features[0]).ToArray();

			var ordered = groupBy.OrderBy(grouping => grouping.Key).ToArray();

			records.Clear();
			foreach (var group in ordered)
			{
				var array = @group.ToArray().OrderBy(record => record.Attribute.Order);
				records.AddRange(array);
			}
		}
	}

	public interface IFeatureFilter
	{
		string[] GetFeatures();

		void Reset();
	}
}