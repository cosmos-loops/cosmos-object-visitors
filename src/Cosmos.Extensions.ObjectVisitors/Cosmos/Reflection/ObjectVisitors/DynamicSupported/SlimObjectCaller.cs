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

namespace Cosmos.Reflection.ObjectVisitors.DynamicSupported
{
    /// <summary>
    /// A slim ObjectCaller for ExpandoObject
    /// </summary>
    public sealed class SlimObjectCaller : ObjectCallerBase
    {
        private SlimObjectTypes _mode;
        private IDictionary<string, object> _expandoObject;
        private dynamic _dynamicObject;

        public override void New() => _expandoObject = new Dictionary<string, object>();

        public void SetInstance(ExpandoObject value)
        {
            _expandoObject = value;
            _dynamicObject = default;
            _mode = SlimObjectTypes.ForExpandoObject;
        }

        public void SetInstance(IDictionary<string, object> value)
        {
            _expandoObject = value;
            _dynamicObject = default;
            _mode = SlimObjectTypes.ForExpandoObject;
        }

        public void SetInstance<T>(T value) where T : DynamicObject
        {
            _dynamicObject = value;
            _expandoObject = default;
            _mode = SlimObjectTypes.ForDynamicObject;
        }

        public IDictionary<string, object> Instance
        {
            get
            {
                return _mode switch
                {
                    SlimObjectTypes.ForExpandoObject => _expandoObject,
                    SlimObjectTypes.ForDynamicObject => ((DynamicObject) _dynamicObject).ToDictionary(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        protected override HashSet<string> InternalMemberNames
        {
            get
            {
                return _mode switch
                {
                    SlimObjectTypes.ForDynamicObject => ((DynamicObject) _dynamicObject).GetDynamicMemberNames().ToHashSet(),
                    SlimObjectTypes.ForExpandoObject => _expandoObject.Keys.ToHashSet(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public override T Get<T>(string name)
        {
            switch (_mode)
            {
                case SlimObjectTypes.ForDynamicObject:
                {
                    var @try = Try.Create(() => (T) _dynamicObject[name]);
                    return @try.GetSafeValue(default(T));
                }

                case SlimObjectTypes.ForExpandoObject:
                {
                    var @try = Try.Create(() => _expandoObject[name].As<T>());
                    return @try.GetSafeValue(default(T));
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Set(string name, object value)
        {
            switch (_mode)
            {
                case SlimObjectTypes.ForDynamicObject:
                    _dynamicObject[name] = value;
                    break;

                case SlimObjectTypes.ForExpandoObject:
                    _expandoObject[name] = value;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetObjInstance(object obj)
        {
            switch (obj)
            {
                case null:
                    return;
                case ExpandoObject expandoObject:
                    SetInstance(expandoObject);
                    break;
                case IObjectVisitor objectVisitor:
                    SetInstance(objectVisitor.ToDictionary());
                    break;
                case DynamicInstance dynamicInstance:
                    SetInstance(dynamicInstance.ExposeVisitor().ToDictionary());
                    break;
                case DynamicObject dynamicObject:
                    SetInstance(dynamicObject);
                    break;
                default:
                    SetInstance((IDictionary<string, object>) obj);
                    break;
            }
        }

        public override object GetObject(string name)
        {
            var @try = _mode switch
            {
                SlimObjectTypes.ForExpandoObject => Try.Create(() => _expandoObject[name]),
                SlimObjectTypes.ForDynamicObject => Try.Create(() => _dynamicObject[name]),
                _ => throw new ArgumentOutOfRangeException()
            };
            return @try.GetSafeValue(default(object));
        }

        public override ObjectMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            switch (_mode)
            {
                case SlimObjectTypes.ForDynamicObject:
                {
                    if (((DynamicObject) _dynamicObject).GetDynamicMemberNames().Contains(name))
                    {
                        var target = _dynamicObject[name];
                        var runtimeType = GetRuntimeType(target);
                        var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
                        return new SlimObjectMember(name, runtimeType, isAsync, _mode);
                    }

                    throw new ArgumentException($"There's no such member named {name}");
                }

                case SlimObjectTypes.ForExpandoObject:
                {
                    if (_expandoObject.TryGetValue(name, out var target))
                    {
                        var runtimeType = GetRuntimeType(target);
                        var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
                        return new SlimObjectMember(name, runtimeType, isAsync, _mode);
                    }

                    throw new ArgumentException($"There's no such member named {name}");
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
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