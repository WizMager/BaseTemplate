using System;

namespace Ecs.Utils.SystemsGenerator
{
    public struct AttributeRecord
    {
        public readonly Type Type;
        public readonly InstallAttribute Attribute;
        public readonly string[] Features;
        public readonly AttributeChanges Changes;

        public AttributeRecord(Type type, InstallAttribute attribute)
        {
            Type = type;
            Attribute = attribute;
            Features = Attribute != null ? Attribute.Features : Array.Empty<string>();
            Changes = new AttributeChanges
            {
                Type = attribute.Type,
                Priority = attribute.Priority,
                Name = string.Join("|", attribute.Features),
                Order = attribute.Order
            };
        }
    }
}