namespace Ecs.Utils.SystemsGenerator.Sort.Impls
{
	public class NameSortStrategy : AStringSortStrategy
	{
		protected override string GetValue(AttributeRecord record) => record.Type.Name;
	}
}