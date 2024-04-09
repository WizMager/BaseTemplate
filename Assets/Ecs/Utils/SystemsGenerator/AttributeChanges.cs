using System;

namespace Ecs.Utils.SystemsGenerator
{
    [Serializable]
    public class AttributeChanges {
        public bool Changed = false;
        public ExecutionType Type;
        public ExecutionPriority Priority;
        public int Order;
        public string Name;
    }
}