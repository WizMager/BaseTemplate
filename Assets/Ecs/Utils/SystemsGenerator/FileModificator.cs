using System;
using System.IO;
using UnityEngine;

namespace Ecs.Utils.SystemsGenerator
{
    public partial class EcsSystemsGeneratorWindow {
		private string FindFileForClass(string typeName) {
			var files = Find(typeName);
			if (files.Length != 1) {
				Debug.LogError($"[EcsInstallerGeneratorWindow] Found {files.Length} occurrences. :(. List is below.");
				Debug.Log(files);
				return "";
			}
			return files[0];
		}

		private string FindFileForSelection(string typeName) {
			var files = Find(typeName);
			if (files.Length != 1) {
				Debug.LogError($"[EcsInstallerGeneratorWindow] Found {files.Length} occurrences. :(. List is below.");
				Debug.Log(files);
				return "";
			}
			return files[0].Replace(Application.dataPath, "Assets");
		}

		private static void ReplaceAttribute(
			string path,
			ExecutionType type,
			string label,
			ExecutionPriority priority,
			int order
		) {
			var file   = File.ReadAllText(path);
			var begin  = file.IndexOf("[Install", StringComparison.Ordinal);
			var end    = file.IndexOf("\n",       begin + 10, StringComparison.Ordinal);
			var source = file.Substring(begin, end - begin);

			var n = label.Length > 0 ? $", \"{label}\"" : "";
			var o = n.Length > 0 || order != 100000 ? $", {order:0000}{n}" : "";

			var destination = $"[Install{type}(ExecutionPriority.{priority}{o})]";
			var result      = file.Replace(source, destination);

			if (File.Exists(path))
				File.Delete(path);
			File.WriteAllText(path, result);

			Debug.Log("[FileModificatorGenerator] Attribute replaced " + path);
		}

		private const string AcmeUsing = "using Acme.InstallerGenerator;\n";

		private static void AddAttribute(
			string path,
			ExecutionType type,
			string label,
			ExecutionPriority priority,
			int order
		) {
			var file = File.ReadAllText(path);

			if (file.Contains("[Install")) {
				ReplaceAttribute(path, type, label, priority, order);
				return;
			}

			var className      = Path.GetFileNameWithoutExtension(path);
			var classNameAt    = file.IndexOf(className, StringComparison.Ordinal);
			var placeForInsert = file.LastIndexOf("\n", classNameAt, StringComparison.Ordinal);

			var n = label.Length > 0 ? $", \"{label}\"" : "";
			var o = n.Length > 0 || order != 100000 ? $", {order:0000}{n}" : "";

			var destination = $"\n\t[Install{type}(ExecutionPriority.{priority}{o})]";
			var result      = file.Insert(placeForInsert, destination);

			if (File.Exists(path))
				File.Delete(path);

			File.WriteAllText(path, !file.Contains(AcmeUsing) ? result.Insert(0, AcmeUsing) : result);
			Debug.Log("[FileModificatorGenerator] Attribute added " + path);
		}

		private static void Remove(string path) {
			var file    = File.ReadAllText(path);
			var rewrite = false;

			if (file.Contains("[Install")) {
				rewrite = true;
				var begin = file.IndexOf("[Install", StringComparison.Ordinal);
				var end   = file.IndexOf("\n", begin + 10, StringComparison.Ordinal) + 1;
				file = file.Remove(begin, end - begin);
			}

			if (file.Contains(AcmeUsing)) {
				rewrite = true;
				file    = file.Replace(AcmeUsing, "");
			}

			if (rewrite) {
				if (File.Exists(path))
					File.Delete(path);

				File.WriteAllText(path, file);
				Debug.Log("[FileModificatorGenerator] Attribute removed " + path);
			}
		}

		private string[] Find(string className)
			=> Directory.GetFiles(Application.dataPath.AddPath(_searchPath), $"{className}.cs", SearchOption.AllDirectories);

		public static void SaveToFile(string text, string filename, string path) {
			var filepath = Application.dataPath.AddPath(path).AddPath(filename);
			if (File.Exists(filepath))
				File.Delete(filepath);
			File.WriteAllText(filepath, text);
		}
	}
}