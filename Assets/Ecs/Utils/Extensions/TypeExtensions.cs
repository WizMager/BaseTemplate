using System;

namespace Ecs.Utils.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasAttribute<T>(this Type type, bool inherit = false) where T : Attribute
        {
            return type.IsDefined(typeof(T), inherit);
        }
    }
}