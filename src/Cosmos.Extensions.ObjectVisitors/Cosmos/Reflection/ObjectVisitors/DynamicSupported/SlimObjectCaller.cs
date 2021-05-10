using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cosmos.Exceptions;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.DynamicSupported
{
    public sealed class SlimObjectCaller : ObjectCallerBase
    {
        private IDictionary<string, object> _dynamicObject;

        public override void New() => _dynamicObject = new Dictionary<string, object>();

        public void SetInstance(ExpandoObject value) => _dynamicObject = value;

        public IDictionary<string, object> Instance => _dynamicObject;

        public override T Get<T>(string name)
        {
            var @try = Try.Create(() => _dynamicObject[name].As<T>());
            return @try.GetSafeValue(default(T));
        }

        public override void Set(string name, object value)
        {
            _dynamicObject[name] = value;
        }

        public override void SetObjInstance(object obj)
        {
            if (obj is null)
                return;
            if (obj is ExpandoObject expandoObject)
                _dynamicObject = expandoObject;
            else if (obj is IObjectVisitor objectVisitor)
                _dynamicObject = objectVisitor.ToDictionary();
            else if (obj is DynamicInstance dynamicInstance)
                _dynamicObject = dynamicInstance.ExposeVisitor().ToDictionary();
            else if (obj is DynamicObject dynamicObject)
                _dynamicObject = dynamicObject.ToDictionary();
            else
                _dynamicObject = (IDictionary<string, object>) obj;
        }

        public override object GetObject(string name)
        {
            var @try = Try.Create(() => _dynamicObject[name]);
            return @try.GetSafeValue(default(object));
        }

        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (_dynamicObject.TryGetValue(name, out var target))
            {
                var runtimeType = GetRuntimeType(target);
                var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
                return new SlimObjectMember(name, runtimeType, isAsync);
            }

            throw new ArgumentException($"There's no such member named {name}");
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