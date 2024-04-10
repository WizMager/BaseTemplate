﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Utils.InstallerGenerator.Models;

namespace Ecs.Utils.InstallerGenerator.CodeGenerators
{
    public static class FeatureInstallerGenerator
    {
        public static string GetInstaller(
            string name,
            string ns,
            string methods,
            string body
        )
        {
            return $@"{ns}
using Zenject; 
using System;
using Entitas.InstallerGenerator2.Impl;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.InstallerGenerator2.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecs.Generated.Installers 
{{
	public static class {name} 
    {{
		public static void Install(DiContainer container, bool isDebug = false)
        {{
{methods}
		}}

{body}	
    }}
}}";
        }

        public static string GetMethodCall(string name)
        {
            return $"\t\t\t{name}(container, isDebug);";
        }

        public static string GetMethodBody(string name, string body)
        {
            return $"\t\tprivate static void {name}(DiContainer container, bool isDebug)\n\t\t{{{body}\n\t\t}}";
        }

        public static string GenerateInstaller(string name, Dictionary<Enum, List<TypeElement>> container, List<string> nameSpaces)
        {
            var nameSpacesSorted = nameSpaces
                .Distinct()
                .Where(nameSpace => !string.IsNullOrWhiteSpace(nameSpace))
                .OrderBy(nameSpace => nameSpace);

            var builtNameSpaces = String.Join("\n", nameSpacesSorted.Select(s => "using " + s + ";"));

            var notEmptyTypes = container.Select(kvp => new
                {
                    methodName = kvp.Key.ToString(),
                    types = kvp.Value,
                    binds = SystemBindGenerator.GetBinds(kvp.Value),
                    installers = SystemBindGenerator.GetInstallerBinds(kvp.Value)
                })
                .Where(s => s.binds.Any())
                .ToList();

            var calls = notEmptyTypes.Select(s => GetMethodCall(s.methodName));
            var builtCalls = string.Join("\n", calls);
            
            var body = notEmptyTypes.Select(s => GetMethodBody(s.methodName, string.Join("\n", s.binds) + "\n\n\n" + string.Join("\n", s.installers)));
            var builtBody = string.Join("\n\n", body);

            return GetInstaller(name, builtNameSpaces, builtCalls, builtBody);
        }
    }
}