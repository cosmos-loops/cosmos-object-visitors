using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Internals.Visitors;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal static class PropertyNodeFactory
    {
        public static IPropertyNodeVisitor Create(ObjectMember member, ObjectCallerBase parentHandlerPtr, ObjectVisitorOptions options)
        {
            void MemberValueSyncHandler(object instance) => parentHandlerPtr[member.MemberName] = instance;
            var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(member.MemberType).AndSetInstance(parentHandlerPtr.GetObject(member.MemberName));
            var visitor = new InstanceVisitor(handler, member.MemberType, options);
            return new PropertyNodeVisitor(member, visitor, options, MemberValueSyncHandler);
        }

        public static IPropertyNodeVisitor Create(
            ObjectMember member, ObjectCallerBase parentHandlerPtr, ObjectVisitorOptions options,
            PropertyNodeVisitor rootVisitor, PropertyNodeVisitor parentVisitor, List<int> rootSignatureCacheRef, List<int> parentSignatureCacheRef, int deep)
        {
            void MemberValueSyncHandler(object instance) => parentHandlerPtr[member.MemberName] = instance;
            var handler = SafeObjectHandleSwitcher.Switch(options.AlgorithmKind)(member.MemberType).AndSetInstance(parentHandlerPtr.GetObject(member.MemberName));
            var visitor = new InstanceVisitor(handler, member.MemberType, options);
            return new PropertyNodeVisitor(member, rootVisitor, parentVisitor, visitor, options, MemberValueSyncHandler, rootSignatureCacheRef, parentSignatureCacheRef, deep + 1);
        }
    }
}