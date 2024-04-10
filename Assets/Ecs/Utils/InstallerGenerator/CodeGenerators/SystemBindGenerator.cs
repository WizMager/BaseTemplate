using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Utils.InstallerGenerator.Models;

namespace Ecs.Utils.InstallerGenerator.CodeGenerators
{
    public static class SystemBindGenerator
    {
        private static string GetEnumValue(Enum e)
        {
            return e.GetType().Name + "." + e;
        }
        
        public static string GetBind(TypeElement type)
        {
            var featuresNames = string.Join("|", type.Features.Select(e => e.ToString()));
            if (type.IsDebug)
            {
                return $@"{"\n\t\t\t"}if(isDebug)
{"\t\t\t\t"}container.BindInterfacesAndSelfTo<{type.Type.Name}>().AsSingle();{$"// {type.Order:0000} {featuresNames}"}";
            }

            return $"\t\t\tcontainer.BindInterfacesAndSelfTo<{type.Type.Name}>().AsSingle();\t// {type.Order:0000} {featuresNames}";
        }

        public static string GetInstallerBind(TypeElement type)
        {
            var featuresNames = string.Join("|", type.Features.Select(e => e.ToString()));

            var features = type.Features.Length > 0 ? ", new Enum[] { " + string.Join(", ", type.Features.Select(GetEnumValue)) + " }" : "";
            if (type.IsDebug)
            {
                return $@"{"\n\t\t\t"}if(isDebug)
{"\t\t\t\t"}container.BindInterfacesAndSelfTo<SystemInstaller<{type.Type.Name}>>().AsSingle()
{"\t\t\t\t\t"}.WithArguments({GetEnumValue(type.FeatureType)}, {GetEnumValue(type.Priority)}, {type.Order}{features});{$"// {type.Order:0000} {featuresNames}"}";
            }

            return $@"{"\t\t\t"}container.BindInterfacesAndSelfTo<SystemInstaller<{type.Type.Name}>>().AsSingle()
{"\t\t\t\t"}.WithArguments({GetEnumValue(type.FeatureType)}, {GetEnumValue(type.Priority)}, {type.Order}{features});{$"// {type.Order:0000} {featuresNames}"}";
        }
        public static IEnumerable<string> GetBinds(IEnumerable<TypeElement> types)
        {
            types = types.OrderBy(t => t.Order).ThenBy(t => t.Type.Name);

            var previous = 100000;
            foreach (var t in types)
            {
                var name = string.Join("|", t.Features.Select(e => e.ToString()));

                if (Math.Abs(previous - t.Order) > 10)
                    yield return $"\n			// {name} {t.Order:0000}";
                yield return GetBind(t);
                previous = t.Order;
            }
        }
        
        public static IEnumerable<string> GetInstallerBinds(IEnumerable<TypeElement> types)
        {
            types = types.OrderBy(t => t.Order).ThenBy(t => t.Type.Name);

            var previous = 100000;
            foreach (var t in types)
            {
                var name = string.Join("|", t.Features.Select(e => e.ToString()));

                if (Math.Abs(previous - t.Order) > 10)
                    yield return $"\n			// {name} {t.Order:0000}";
                yield return GetInstallerBind(t);
                previous = t.Order;
            }
        }
    }
}