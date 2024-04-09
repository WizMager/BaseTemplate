using System.Collections.Generic;

namespace Ecs.Utils.SystemsGenerator.Sort
{
	public interface ISortStrategy
	{
		string Name { get; }

		void Next();

		void Reset();

		void Sort(List<AttributeRecord> records);
	}
}