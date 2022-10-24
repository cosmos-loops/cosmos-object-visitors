using System.Reflection;
using System.Runtime.CompilerServices;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Validation.Objects;
#if NET452 || NET462
using FastMember;

#endif

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.TupleServices
{
    /// <summary>
    /// Tuple service slim for Object Member <br />
    /// 元组服务模拟
    /// </summary>
    public sealed class TupleServiceSlimObjectMember : ObjectMember
    {
        public TupleServiceSlimObjectMember(
            string name,
            Type type,
            bool isAsync
        ) : base(true, true, false, name, type, false, isAsync, false, false, false, false, false, VerifiableMemberKind.Field) { }

        /// <summary>
        /// Slim for ...
        /// </summary>
        public SlimSupportedFor SlimFor => SlimSupportedFor.Tuple;

        internal static TupleServiceSlimObjectMember Of(FieldInfo fi)
        {
            var runtimeType = fi.FieldType;
            var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
            return new(fi.Name, runtimeType, isAsync);
        }

        internal static TupleServiceSlimObjectMember Of(PropertyInfo pi)
        {
            var runtimeType = pi.PropertyType;
            var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
            return new(pi.Name, runtimeType, isAsync);
        }

#if NET452 || NET462
        internal static TupleServiceSlimObjectMember Of(Member member)
        {
            var runtimeType = member.Type;
            var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
            return new(member.Name, runtimeType, isAsync);
        }
#endif
    }
}