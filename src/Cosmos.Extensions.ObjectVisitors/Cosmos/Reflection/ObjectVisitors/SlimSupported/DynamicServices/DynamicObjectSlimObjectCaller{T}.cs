using System.Dynamic;
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

#if NET452 || NET462
using Cosmos.Collections;
#endif

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    /// <summary>
    /// Dynamic Object Slim for Object Caller <br />
    /// 动态对象模拟
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DynamicObjectSlimObjectCaller<T> : ObjectCallerBase<T>
        where T : DynamicObject, new()
    {
        public SlimSupportedFor Mode => SlimSupportedFor.DynamicObject;

        public override void New() => Instance = new T();

        protected override HashSet<string> InternalMemberNames => Instance.GetDynamicMemberNames().ToHashSet();

        public override TMember Get<TMember>(string name)
        {
            return (TMember) GetObject(name);
        }

        public override void Set(string name, object value)
        {
            ((dynamic) Instance)[name] = value;
        }

        public override object GetObject(string name)
        {
            var @try = Try.Create(() => ((dynamic) Instance)[name]);
            return @try.GetSafeValue(default(object));
        }

        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (!Instance.GetDynamicMemberNames().Contains(name))
                throw new ArgumentException($"There's no such member named {name}");

            var target = ((dynamic) Instance)[name];
            var runtimeType = GetRuntimeType(target);
            var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
            return new DynamicServiceSlimObjectMember(name, runtimeType, isAsync, Mode);
        }

        private static Type GetRuntimeType(object target)
        {
            if (target is null)
                throw new InvalidOperationException("The Target value is null, and the runtime type cannot be obtained.");

            return U(target);

            Type U<T2>(T2 t) => typeof(T2);
        }
    }
}