using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Internals.Visitors;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals.PropertyNodes
{
    internal static class PropertyNodeFactory
    {
        public static IPropertyNodeVisitor Create(ObjectMember member, ObjectCallerBase parentHandlerPtr, AlgorithmKind kind, bool repeatable, bool strictMode)
        {
            void MemberValueSyncHandler(object instance) => parentHandlerPtr[member.MemberName] = instance;
            var handler = SafeObjectHandleSwitcher.Switch(kind)(member.MemberType).AndSetInstance(parentHandlerPtr.GetObject(member.MemberName));
            var visitor = new InstanceVisitor(handler, member.MemberType, kind, repeatable, LvMode.FULL, strictMode);
            return new PropertyNodeVisitor(member, visitor, (kind, repeatable, strictMode), MemberValueSyncHandler);
        }

        public static IPropertyNodeVisitor Create(
            ObjectMember member, ObjectCallerBase parentHandlerPtr, AlgorithmKind kind, bool repeatable, bool strictMode,
            PropertyNodeVisitor rootVisitor, PropertyNodeVisitor parentVisitor, List<int> rootSignatureCacheRef, List<int> parentSignatureCacheRef, int deep)
        {
            void MemberValueSyncHandler(object instance) => parentHandlerPtr[member.MemberName] = instance;
            var handler = SafeObjectHandleSwitcher.Switch(kind)(member.MemberType).AndSetInstance(parentHandlerPtr.GetObject(member.MemberName));
            var visitor = new InstanceVisitor(handler, member.MemberType, kind, repeatable, LvMode.FULL, strictMode);
            return new PropertyNodeVisitor(member, rootVisitor, parentVisitor, visitor, (kind, repeatable, strictMode), MemberValueSyncHandler, rootSignatureCacheRef, parentSignatureCacheRef, deep + 1);
        }
    }
}