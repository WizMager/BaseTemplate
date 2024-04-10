using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ecs.Utils.Extensions;
using Ecs.Utils.InstallerGenerator.Attributes;
using Ecs.Utils.InstallerGenerator.CodeGenerators;
using Ecs.Utils.InstallerGenerator.Models;
using UnityEditor;
using UnityEngine;

namespace Ecs.Utils.InstallerGenerator
{
    public class EcsInstallerGenerator
    {
        private static bool _inProgress;

        [MenuItem("Tools/Entitas/Generate Installers &g")]
        public static void GenerateManual()
        {
            if (_inProgress)
                return;

            EditorUtility.DisplayProgressBar("Ecs Installer", "Generate...", .1f);

            try
            {
                _inProgress = true;
                var installerTemplates = Generate();
                const string path = "Ecs/Installers/Generated/";
                
                var directoryPath = Path.Combine(Application.dataPath, path);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                foreach (var template in installerTemplates)
                {
                    template.SaveToFile(directoryPath);
                }
            }
            finally
            {
                _inProgress = false;
                EditorUtility.ClearProgressBar();
            }

            AssetDatabase.Refresh();
        }

        public static EcsInstallerTemplate[] Generate()
        {
            var ecsInstallers = CollectTemplates();

            foreach (var template in ecsInstallers)
            {
                var generatedInstallerCode = FeatureInstallerGenerator.GenerateInstaller(template.Name, template.Container, template.Namespaces);
                template.GeneratedInstallerCode = generatedInstallerCode;
                Debug.Log($"[EcsInstallerGenerator] Generated {template.Type}: {template.Counter}");
            }

            var installerTemplates = ecsInstallers.ToArray();
            return installerTemplates;
        }

        private static List<EcsInstallerTemplate> CollectTemplates()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes())
                .Where(type => type.HasAttribute<AInstallAttribute>() && !type.HasAttribute<IgnoreAttribute>());

            var ecsInstallers = new List<EcsInstallerTemplate>();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute(typeof(AInstallAttribute), false) as AInstallAttribute;
                var isDebug = type.HasAttribute<DebugSystemAttribute>();

                var featureType = attribute!.FeatureType;

                if (!ecsInstallers.Exists(i => i.Type.GetType() == featureType.GetType()))
                {
                    var featureTypeValues = Enum.GetValues(featureType.GetType()).Cast<Enum>();
                    foreach (var featureTypeValue in featureTypeValues)
                    {
                        var template = new EcsInstallerTemplate(featureTypeValue);
                        ecsInstallers.Add(template);
                    }
                }

                foreach (var installerTemplate in ecsInstallers.Where(i => i.Type.GetType() == featureType.GetType()))
                {
                    if (!featureType.Equals(installerTemplate.Type))
                        continue;

                    installerTemplate.Counter++;

                    if (!installerTemplate.Container.ContainsKey(attribute.Priority))
                        installerTemplate.Container[attribute.Priority] = new();

                    installerTemplate.Container[attribute.Priority].Add(new(type, attribute, isDebug));

                    installerTemplate.Namespaces.Add(type.Namespace);
                    
                    installerTemplate.Namespaces.Add(attribute.FeatureType.GetType().Namespace);
                    installerTemplate.Namespaces.Add(attribute.Priority.GetType().Namespace);
                    foreach (var feature in attribute.Features)
                    {
                        installerTemplate.Namespaces.Add(feature.GetType().Namespace);
                    }
                }
            }

            return ecsInstallers;
        }
    }
}