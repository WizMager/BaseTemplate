using System;
using System.Collections.Generic;
using System.IO;

namespace Ecs.Utils.InstallerGenerator.Models
{
	public class EcsInstallerTemplate
	{
		public readonly Enum Type;
		public readonly Dictionary<Enum, List<TypeElement>> Container;
		public readonly List<string> Namespaces = new();

		public string Name => $"{Type}EcsSystems";

		public string GeneratedInstallerCode;
		public int Counter;


		public EcsInstallerTemplate(Enum type)
		{
			Type = type;
			Container = new();
		}
		
		public void SaveToFile(string path)
		{
			var filepath = Path.Combine(path, Name) + ".cs";
			if (File.Exists(filepath))
				File.Delete(filepath);
			File.WriteAllText(filepath, GeneratedInstallerCode);
		}
	}
}