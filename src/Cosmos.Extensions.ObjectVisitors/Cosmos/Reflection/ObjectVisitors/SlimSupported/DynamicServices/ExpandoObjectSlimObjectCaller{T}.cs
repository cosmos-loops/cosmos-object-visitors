using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
#if NET452 || NET462
using Cosmos.Collections;
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;
#else
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

#endif

namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices
{
    public sealed class ExpandoObjectSlimObjectCaller : ObjectCallerBase<ExpandoObject>
    {
        public SlimSupportedFor Mode => SlimSupportedFor.ExpandoObject;

        public override void New() => Instance = new ExpandoObject();

        protected override HashSet<string> InternalMemberNames => ((IDictionary<string, object>) Instance).Keys.ToHashSet();

        public override T Get<T>(string name)
        {
            return (T) GetObject(name);
        }

        public override void Set(string name, object value)
        {
            ((IDictionary<string, object>) Instance)[name] = value;
        }

        public override object GetObject(string name)
        {
            var @try = Try.Create(() => ((IDictionary<string, object>) Instance)[name]);
            return @try.GetSafeValue(default(object));
        }

        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (!((IDictionary<string, object>) Instance).Keys.Contains(name))
                throw new ArgumentException($"There's no such member named {name}");

            var target = ((IDictionary<string, object>) Instance)[name];
            var runtimeType = GetRuntimeType(target);
            var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
            return new DynamicServiceSlimObjectMember(name, runtimeType, isAsync, Mode);
        }

        private static Type GetRuntimeType(object target)
        {
            if (target is null)
                throw new InvalidOperationException("The Target value is null, and the runtime type cannot be obtained.");

            return U(target);

            Type U<T>(T t) => typeof(T);
        }
    }
}