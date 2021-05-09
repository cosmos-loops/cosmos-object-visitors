using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors
{
    public class ObjectLoopContext
    {
        public string Name { get; }

        public object Value { get; }

        public ObjectMember Metadata { get; }

        public int Index { get; }

        public ObjectLoopContext(string name, object value, ObjectMember member, int index)
        {
            Name = name;
            Value = value;
            Metadata = member;
            Index = index;
        }
    }
}