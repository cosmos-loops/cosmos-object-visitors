// using System;
// using System.Collections.Generic;
// using System.Dynamic;
// using System.Linq;
// using System.Reflection;
// using System.Runtime.CompilerServices;
// #if NET452 || NET462
// using Cosmos.Collections;
// using Cosmos.Exceptions;
// using Cosmos.Reflection.ObjectVisitors.Core;
// using Cosmos.Reflection.ObjectVisitors.Metadata;
// #else
// using Cosmos.Exceptions;
// using Cosmos.Reflection.ObjectVisitors.Core;
// using Cosmos.Reflection.ObjectVisitors.Metadata;
//
// #endif
//
// namespace Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices
// {
//     /// <summary>
//     /// A slim ObjectCaller for ExpandoObject/DynamicObject
//     /// </summary>
//     public sealed class DynamicServiceSlimObjectCaller : ObjectCallerBase
//     {
//        
//         
//         private SlimSupportedFor _mode;
//         private IDictionary<string, object> _expandoObject;
//         private dynamic _dynamicObject;
//
//         public override void New() => _expandoObject = new Dictionary<string, object>();
//
//         public void SetInstance(ExpandoObject value)
//         {
//             _expandoObject = value;
//             _dynamicObject = default;
//             _mode = SlimSupportedFor.ExpandoObject;
//         }
//
//         public void SetInstance(IDictionary<string, object> value)
//         {
//             _expandoObject = value;
//             _dynamicObject = default;
//             _mode = SlimSupportedFor.ExpandoObject;
//         }
//
//         public void SetInstance<T>(T value) where T : DynamicObject
//         {
//             _dynamicObject = value;
//             _expandoObject = default;
//             _mode = SlimSupportedFor.DynamicObject;
//         }
//
//         public IDictionary<string, object> Instance
//         {
//             get
//             {
//                 return _mode switch
//                 {
//                     SlimSupportedFor.ExpandoObject => _expandoObject,
//                     SlimSupportedFor.DynamicObject => ((DynamicObject) _dynamicObject).ToDictionary(),
//                     _ => throw new ArgumentOutOfRangeException()
//                 };
//             }
//         }
//
//         protected override HashSet<string> InternalMemberNames
//         {
//             get
//             {
//                 return _mode switch
//                 {
//                     SlimSupportedFor.DynamicObject => ((DynamicObject) _dynamicObject).GetDynamicMemberNames().ToHashSet(),
//                     SlimSupportedFor.ExpandoObject => _expandoObject.Keys.ToHashSet(),
//                     _ => throw new ArgumentOutOfRangeException()
//                 };
//             }
//         }
//
//         internal HashSet<string> InternalMemberNamesPtr => InternalMemberNames;
//
//         public override T Get<T>(string name)
//         {
//             return (T) GetObject(name);
//         }
//
//         public override void Set(string name, object value)
//         {
//             switch (_mode)
//             {
//                 case SlimSupportedFor.DynamicObject:
//                     _dynamicObject[name] = value;
//                     break;
//
//                 case SlimSupportedFor.ExpandoObject:
//                     _expandoObject[name] = value;
//                     break;
//
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//         }
//
//         public override void SetObjInstance(object obj)
//         {
//             switch (obj)
//             {
//                 case null:
//                     return;
//                 case ExpandoObject expandoObject:
//                     SetInstance(expandoObject);
//                     break;
//                 case IObjectVisitor objectVisitor:
//                     SetInstance(objectVisitor.ToDictionary());
//                     break;
//                 case DynamicInstance dynamicInstance:
//                     SetInstance(dynamicInstance.ExposeVisitor().ToDictionary());
//                     break;
//                 case DynamicObject dynamicObject:
//                     SetInstance(dynamicObject);
//                     break;
//                 default:
//                     SetInstance((IDictionary<string, object>) obj);
//                     break;
//             }
//         }
//
//         public override object GetObjInstance()
//         {
//             return _mode switch
//             {
//                 SlimSupportedFor.ExpandoObject => _expandoObject,
//                 SlimSupportedFor.DynamicObject => _dynamicObject,
//                 _ => _expandoObject
//             };
//         }
//
//         public override object GetObject(string name)
//         {
//             var @try = _mode switch
//             {
//                 SlimSupportedFor.ExpandoObject => Try.Create(() => _expandoObject[name]),
//                 SlimSupportedFor.DynamicObject => Try.Create(() => _dynamicObject[name]),
//                 _ => throw new ArgumentOutOfRangeException()
//             };
//             return @try.GetSafeValue(default(object));
//         }
//
//         public override ObjectMember GetMember(string name)
//         {
//             if (string.IsNullOrWhiteSpace(name))
//                 throw new ArgumentNullException(nameof(name));
//
//             switch (_mode)
//             {
//                 case SlimSupportedFor.DynamicObject:
//                 {
//                     if (((DynamicObject) _dynamicObject).GetDynamicMemberNames().Contains(name))
//                     {
//                         var target = _dynamicObject[name];
//                         var runtimeType = GetRuntimeType(target);
//                         var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
//                         return new DynamicServiceSlimObjectMember(name, runtimeType, isAsync, _mode);
//                     }
//
//                     throw new ArgumentException($"There's no such member named {name}");
//                 }
//
//                 case SlimSupportedFor.ExpandoObject:
//                 {
//                     if (_expandoObject.TryGetValue(name, out var target))
//                     {
//                         var runtimeType = GetRuntimeType(target);
//                         var isAsync = runtimeType?.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) is not null;
//                         return new DynamicServiceSlimObjectMember(name, runtimeType, isAsync, _mode);
//                     }
//
//                     throw new ArgumentException($"There's no such member named {name}");
//                 }
//
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//         }
//
//         private static Type GetRuntimeType(object target)
//         {
//             if (target is null)
//                 throw new InvalidOperationException("The Target value is null, and the runtime type cannot be obtained.");
//
//             return U(target);
//
//             Type U<T>(T t) => typeof(T);
//         }
//     }
// }