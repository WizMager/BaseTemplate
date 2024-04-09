using System;
using System.Collections.Generic;

namespace Ecs.Utils.SystemsGenerator
{
    public class EcsSystemTemplate
    {
        public readonly ExecutionType Type;
        public readonly Dictionary<ExecutionPriority, List<TypeElement>> Container;
        public readonly List<string> Namespaces = new();

        public string Name => $"{Type}EcsSystems";

        public string GeneratedInstallerCode;
        public int Counter;


        public EcsSystemTemplate(ExecutionType type)
        {
            Type = type;
            Container = GetContainer();
        }

        private static Dictionary<ExecutionPriority, List<TypeElement>> GetContainer()
        {
            var priorities = Enum.GetValues(typeof(ExecutionPriority)) as ExecutionPriority[];
            var dictionary = new Dictionary<ExecutionPriority, List<TypeElement>>();
            foreach (var priority in priorities)
            {
                dictionary.Add(priority, new List<TypeElement>());
            }

            return dictionary;
        }

        protected bool Equals(EcsSystemTemplate other)
        {
            return Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EcsSystemTemplate)obj);
        }

        public override int GetHashCode()
        {
            return (int)Type;
        }
    }
}