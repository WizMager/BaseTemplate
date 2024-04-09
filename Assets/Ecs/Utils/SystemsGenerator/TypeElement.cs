using System;

namespace Ecs.Utils.SystemsGenerator
{
    public class TypeElement
    {
        public readonly Type Type;
        public readonly int Order;
        public readonly string Name;

        public TypeElement(Type type, InstallAttribute attribute)
        {
            Type = type;
            Order = attribute.Order;
            Name = string.Join("|", attribute.Features);
        }
    }
}