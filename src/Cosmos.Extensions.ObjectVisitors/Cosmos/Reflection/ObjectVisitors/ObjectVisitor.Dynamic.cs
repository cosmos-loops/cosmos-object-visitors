using System;
using System.Collections.Generic;
using System.Dynamic;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Internals.Visitors;
using Cosmos.Reflection.ObjectVisitors.Metadata;
using Cosmos.Reflection.ObjectVisitors.SlimSupported.DynamicServices;

namespace Cosmos.Reflection.ObjectVisitors
{
    public static partial class ObjectVisitor
    {
        public static class Dynamic
        {
            #region Create for ExpandoObject

            public static IObjectVisitor CreateForExpandoObject(ExpandoObject instance, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var type = typeof(ExpandoObject);
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = ((ExpandoObjectSlimObjectCaller) DynamicServiceTypeHelper.Create(type)).AndSetExpandoObject(instance);
                return new InstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForExpandoObject(ExpandoObject instance, ObjectVisitorOptions options)
            {
                var type = typeof(ExpandoObject);
                var handler = ((ExpandoObjectSlimObjectCaller) DynamicServiceTypeHelper.Create(type)).AndSetExpandoObject(instance);
                return new InstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForExpandoObject(bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var type = typeof(ExpandoObject);
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = (ExpandoObjectSlimObjectCaller) DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForExpandoObject(ObjectVisitorOptions options)
            {
                var type = typeof(ExpandoObject);
                var handler = (ExpandoObjectSlimObjectCaller) DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForExpandoObject(IDictionary<string, object> initialValues, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var type = typeof(ExpandoObject);
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = (ExpandoObjectSlimObjectCaller) DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            public static IObjectVisitor CreateForExpandoObject(IDictionary<string, object> initialValues, ObjectVisitorOptions options)
            {
                var type = typeof(ExpandoObject);
                var handler = (ExpandoObjectSlimObjectCaller) DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            #endregion

            #region Create for DynamicObject

            public static IObjectVisitor CreateForDynamicObject(Type type, DynamicObject instance, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create(type).AndSetDynamicObject(instance);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new InstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForDynamicObject<T>(T instance, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
                where T : DynamicObject, new()
            {
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create<T>().AndSetDynamicObject(instance);
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new InstanceVisitor<T>(handler, options);
            }

            public static IObjectVisitor CreateForDynamicObject(Type type, DynamicObject instance, ObjectVisitorOptions options)
            {
                var handler = DynamicServiceTypeHelper.Create(type).AndSetDynamicObject(instance);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new InstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForDynamicObject<T>(T instance, ObjectVisitorOptions options)
                where T : DynamicObject, new()
            {
                var handler = DynamicServiceTypeHelper.Create<T>().AndSetDynamicObject(instance);
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new InstanceVisitor<T>(handler, options);
            }

            public static IObjectVisitor CreateForDynamicObject(Type type, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForDynamicObject<T>(bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
                where T : DynamicObject, new()
            {
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options);
            }

            public static IObjectVisitor CreateForDynamicObject(Type type, ObjectVisitorOptions options)
            {
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options);
            }

            public static IObjectVisitor CreateForDynamicObject<T>(ObjectVisitorOptions options)
                where T : DynamicObject, new()
            {
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options);
            }

            public static IObjectVisitor CreateForDynamicObject(Type type, IDictionary<string, object> initialValues, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            public static IObjectVisitor CreateForDynamicObject<T>(IDictionary<string, object> initialValues, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
                where T : DynamicObject, new()
            {
                var options = FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options, initialValues);
            }

            public static IObjectVisitor CreateForDynamicObject(Type type, IDictionary<string, object> initialValues, ObjectVisitorOptions options)
            {
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            public static IObjectVisitor CreateForDynamicObject<T>(IDictionary<string, object> initialValues, ObjectVisitorOptions options)
                where T : DynamicObject, new()
            {
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options, initialValues);
            }

            #endregion

            #region Create for DynamicInstance

            public static IObjectVisitor Create(DynamicInstance instance)
            {
                if (instance is null)
                    throw new ArgumentNullException(nameof(instance));
                return instance.ExposeVisitor();
            }

            public static IObjectVisitor<T> Create<T>(DynamicInstance<T> instance)
            {
                if (instance is null)
                    throw new ArgumentNullException(nameof(instance));
                return instance.ExposeVisitor();
            }

            #endregion
        }
    }
}