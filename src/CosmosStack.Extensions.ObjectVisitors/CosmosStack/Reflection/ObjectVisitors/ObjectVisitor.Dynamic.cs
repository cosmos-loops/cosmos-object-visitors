using System;
using System.Collections.Generic;
using System.Dynamic;
using CosmosStack.Reflection.ObjectVisitors.Internals;
using CosmosStack.Reflection.ObjectVisitors.Internals.Visitors;
using CosmosStack.Reflection.ObjectVisitors.Metadata;
using CosmosStack.Reflection.ObjectVisitors.SlimSupported.DynamicServices;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// Object Visitor <br />
    /// 对象访问器
    /// </summary>
    public static partial class ObjectVisitor
    {
        /// <summary>
        /// Dynamic utilities <br />
        /// 动态工具
        /// </summary>
        public static class Dynamic
        {
            #region Create for ExpandoObject

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 ExpandoObject 创建对象访问器
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForExpandoObject(ExpandoObject instance, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var type = typeof(ExpandoObject);
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = ((ExpandoObjectSlimObjectCaller)DynamicServiceTypeHelper.Create(type)).AndSetExpandoObject(instance);
                return new InstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 ExpandoObject 创建对象访问器
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForExpandoObject(ExpandoObject instance, ObjectVisitorOptions options)
            {
                var type = typeof(ExpandoObject);
                var handler = ((ExpandoObjectSlimObjectCaller)DynamicServiceTypeHelper.Create(type)).AndSetExpandoObject(instance);
                return new InstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 ExpandoObject 创建对象访问器
            /// </summary>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForExpandoObject(bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var type = typeof(ExpandoObject);
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = (ExpandoObjectSlimObjectCaller)DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 ExpandoObject 创建对象访问器
            /// </summary>
            /// <param name="options"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForExpandoObject(ObjectVisitorOptions options)
            {
                var type = typeof(ExpandoObject);
                var handler = (ExpandoObjectSlimObjectCaller)DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 ExpandoObject 创建对象访问器
            /// </summary>
            /// <param name="initialValues"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForExpandoObject(IDictionary<string, object> initialValues, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var type = typeof(ExpandoObject);
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = (ExpandoObjectSlimObjectCaller)DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 ExpandoObject 创建对象访问器
            /// </summary>
            /// <param name="initialValues"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForExpandoObject(IDictionary<string, object> initialValues, ObjectVisitorOptions options)
            {
                var type = typeof(ExpandoObject);
                var handler = (ExpandoObjectSlimObjectCaller)DynamicServiceTypeHelper.Create(type);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            #endregion

            #region Create for DynamicObject

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="type"></param>
            /// <param name="instance"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject(Type type, DynamicObject instance, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create(type).AndSetDynamicObject(instance);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new InstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject<T>(T instance, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
                where T : DynamicObject, new()
            {
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create<T>().AndSetDynamicObject(instance);
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new InstanceVisitor<T>(handler, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="type"></param>
            /// <param name="instance"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject(Type type, DynamicObject instance, ObjectVisitorOptions options)
            {
                var handler = DynamicServiceTypeHelper.Create(type).AndSetDynamicObject(instance);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new InstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="options"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject<T>(T instance, ObjectVisitorOptions options)
                where T : DynamicObject, new()
            {
                var handler = DynamicServiceTypeHelper.Create<T>().AndSetDynamicObject(instance);
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new InstanceVisitor<T>(handler, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="type"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject(Type type, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject<T>(bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
                where T : DynamicObject, new()
            {
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="type"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject(Type type, ObjectVisitorOptions options)
            {
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="options"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject<T>(ObjectVisitorOptions options)
                where T : DynamicObject, new()
            {
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="type"></param>
            /// <param name="initialValues"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject(Type type, IDictionary<string, object> initialValues, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
            {
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="initialValues"></param>
            /// <param name="repeatable"></param>
            /// <param name="strictMode"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject<T>(IDictionary<string, object> initialValues, bool repeatable = RpMode.REPEATABLE, bool strictMode = StMode.NORMALE)
                where T : DynamicObject, new()
            {
                var options = ObjectVisitor.FillWith(AlgorithmKind.Precision, repeatable, strictMode);
                var handler = DynamicServiceTypeHelper.Create<T>();
                var type = typeof(T);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor<T>(handler, options);
                return new FutureInstanceVisitor<T>(handler, options, initialValues);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="type"></param>
            /// <param name="initialValues"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public static IObjectVisitor CreateForDynamicObject(Type type, IDictionary<string, object> initialValues, ObjectVisitorOptions options)
            {
                var handler = DynamicServiceTypeHelper.Create(type);
                if (type.IsAbstract && type.IsSealed)
                    return new StaticTypeObjectVisitor(handler, type, options);
                return new FutureInstanceVisitor(handler, type, options, initialValues);
            }

            /// <summary>
            /// Create for ExpandoObject <br />
            /// 为 DynamicObject 创建对象访问器
            /// </summary>
            /// <param name="initialValues"></param>
            /// <param name="options"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
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

            /// <summary>
            /// Create for <see cref="DynamicInstance"/> <br />
            /// 为动态实例 <see cref="DynamicInstance"/> 创建对象访问器
            /// </summary>
            /// <param name="instance"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            public static IObjectVisitor Create(DynamicInstance instance)
            {
                if (instance is null)
                    throw new ArgumentNullException(nameof(instance));
                return instance.ExposeVisitor();
            }

            /// <summary>
            /// Create for <see cref="DynamicInstance{T}"/> <br />
            /// 为动态实例 <see cref="DynamicInstance{T}"/> 创建对象访问器
            /// </summary>
            /// <param name="instance"></param>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
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