using System;
using System.Collections.Generic;
using CosmosStack.Reflection.ObjectVisitors.Internals;
using CosmosStack.Reflection.ObjectVisitors.Internals.Loop;
using CosmosStack.Reflection.ObjectVisitors.Internals.Repeat;
using CosmosStack.Reflection.ObjectVisitors.Internals.Select;
using CosmosStack.Reflection.ObjectVisitors.Metadata;

namespace CosmosStack.Reflection.ObjectVisitors
{
    /// <summary>
    /// Object Visitor Extensions <br />
    /// 对象访问器扩展
    /// </summary>
    public static class ObjectVisitorExtensions
    {
        #region To Visitor

        /// <summary>
        /// Convert an object into an Object Visitor <br />
        /// 将一个对象实例转换为对象访问器
        /// </summary>
        /// <param name="instanceObj"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IObjectVisitor<T> ToVisitor<T>(this T instanceObj,
            AlgorithmKind kind = AlgorithmKind.Precision,
            bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE)
            where T : class
        {
            return ObjectVisitor.Create(instanceObj, kind, repeatable, strictMode);
        }

        /// <summary>
        /// Convert a type into an Object Visitor <br />
        /// 将一个类型转换为对象访问器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <returns></returns>
        public static IObjectVisitor ToVisitor(this Type type,
            AlgorithmKind kind = AlgorithmKind.Precision,
            bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE)
        {
            return ObjectVisitor.Create(type, kind, repeatable, strictMode);
        }

        /// <summary>
        /// Convert a dictionary into an Object Visitor according to the given type <br />
        /// 将一个字典根据给定的类型转换为一个对象访问器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="initialValues"></param>
        /// <param name="kind"></param>
        /// <param name="repeatable"></param>
        /// <param name="strictMode"></param>
        /// <returns></returns>
        public static IObjectVisitor ToVisitor(this Type type, IDictionary<string, object> initialValues,
            AlgorithmKind kind = AlgorithmKind.Precision,
            bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE)
        {
            return ObjectVisitor.Create(type, initialValues, kind, repeatable, strictMode);
        }

        #endregion

        #region ToDynamicInstance

        /// <summary>
        /// Convert an Object Visitor into a <see cref="DynamicInstance"/> instance <br />
        /// 将一个对象访问器转换为动态实例 <see cref="DynamicInstance"/>
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public static DynamicInstance ToDynamicObject(this IObjectVisitor visitor)
        {
            return new(visitor);
        }

        /// <summary>
        /// Convert an Object Visitor into a <see cref="DynamicInstance{T}"/> instance <br />
        /// 将一个对象访问器转换为动态实例 <see cref="DynamicInstance{T}"/>
        /// </summary>
        /// <param name="visitor"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DynamicInstance<T> ToDynamicObject<T>(this IObjectVisitor<T> visitor)
        {
            return new(visitor);
        }

        #endregion

        #region To Dictionary

        /// <summary>
        /// Convert an Object Visitor into a dictionary <br />
        /// 将一个对象访问器转换为字典
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Dictionary<string, object> ToDictionary(this IObjectVisitor visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var val = new Dictionary<string, object>();
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            var lazyHandler = rel.ExposeLazyMemberHandler();
            foreach (var name in lazyHandler.Value.GetNames())
                val[name] = visitor.GetValue(name);
            return val;
        }

        /// <summary>
        /// Convert an Object Visitor into a dictionary <br />
        /// 将一个对象访问器转换为字典
        /// </summary>
        /// <param name="visitor"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Dictionary<string, object> ToDictionary<T>(this IObjectVisitor<T> visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var val = new Dictionary<string, object>();
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            var lazyHandler = rel.ExposeLazyMemberHandler();
            foreach (var name in lazyHandler.Value.GetNames())
                val[name] = visitor.GetValue(name);
            return val;
        }

        #endregion

        #region Select

        /// <summary>
        /// Select <br />
        /// 选择
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopFunc"></param>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectSelector<TVal> Select<TVal>(this IObjectVisitor visitor, Func<string, object, ObjectMember, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        /// <summary>
        /// Select <br />
        /// 选择
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopFunc"></param>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectSelector<TVal> Select<TVal>(this IObjectVisitor visitor, Func<string, object, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        /// <summary>
        /// Select <br />
        /// 选择
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopFunc"></param>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectSelector<TVal> Select<TVal>(this IObjectVisitor visitor, Func<ObjectLoopContext, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        /// <summary>
        /// Select <br />
        /// 选择
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectSelector<T, TVal> Select<T, TVal>(this IObjectVisitor<T> visitor, Func<string, object, ObjectMember, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<T, TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        /// <summary>
        /// Select <br />
        /// 选择
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectSelector<T, TVal> Select<T, TVal>(this IObjectVisitor<T> visitor, Func<string, object, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<T, TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        /// <summary>
        /// Select <br />
        /// 选择
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectSelector<T, TVal> Select<T, TVal>(this IObjectVisitor<T> visitor, Func<ObjectLoopContext, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<T, TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        #endregion

        #region For Each

        /// <summary>
        /// ForEach <br />
        /// 循环
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopAct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectLooper ForEach(this IObjectVisitor visitor, Action<string, object, ObjectMember> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        /// <summary>
        /// ForEach <br />
        /// 循环
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopAct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectLooper ForEach(this IObjectVisitor visitor, Action<string, object> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        /// <summary>
        /// ForEach <br />
        /// 循环
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopAct"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectLooper ForEach(this IObjectVisitor visitor, Action<ObjectLoopContext> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        /// <summary>
        /// ForEach <br />
        /// 循环
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopAct"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectLooper<T> ForEach<T>(this IObjectVisitor<T> visitor, Action<string, object, ObjectMember> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper<T>(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        /// <summary>
        /// ForEach <br />
        /// 循环
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopAct"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectLooper<T> ForEach<T>(this IObjectVisitor<T> visitor, Action<string, object> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper<T>(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        /// <summary>
        /// ForEach <br />
        /// 循环
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="loopAct"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IObjectLooper<T> ForEach<T>(this IObjectVisitor<T> visitor, Action<ObjectLoopContext> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper<T>(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        #endregion

        #region For Repeat

        /// <summary>
        /// Repeat <br />
        /// 重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IObjectRepeater ForRepeat(this IObjectVisitor visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            if (visitor.IsStatic) return new EmptyRepeater(visitor.SourceType);
            var rel = (ICoreVisitor)visitor;
            if (rel.ExposeHistoricalContext() is null) return new EmptyRepeater(visitor.SourceType);
            return new ObjectRepeater(rel.ExposeHistoricalContext());
        }

        /// <summary>
        /// Repeat <br />
        /// 重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IObjectRepeater<T> ForRepeat<T>(this IObjectVisitor<T> visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            if (visitor.IsStatic) return new EmptyRepeater<T>();
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.ExposeHistoricalContext() is null) return new EmptyRepeater<T>();
            return new ObjectRepeater<T>(rel.ExposeHistoricalContext());
        }

        #endregion

        #region Try Repeat

        /// <summary>
        /// Try to repeat <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryRepeat(this IObjectVisitor visitor, out object result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor)visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat();
            return true;
        }

        /// <summary>
        /// Try to repeat <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryRepeat(this IObjectVisitor visitor, object instance, out object result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor)visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(instance);
            return true;
        }

        /// <summary>
        /// Try to repeat <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="keyValueCollections"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryRepeat(this IObjectVisitor visitor, IDictionary<string, object> keyValueCollections, out object result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor)visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(keyValueCollections);
            return true;
        }

        /// <summary>
        /// Try to repeat <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryRepeat<T>(this IObjectVisitor<T> visitor, out T result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat();
            return true;
        }

        /// <summary>
        /// Try to repeat <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryRepeat<T>(this IObjectVisitor<T> visitor, T instance, out T result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(instance);
            return true;
        }

        /// <summary>
        /// Try to repeat <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="keyValueCollections"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool TryRepeat<T>(this IObjectVisitor<T> visitor, IDictionary<string, object> keyValueCollections, out T result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor<T>)visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(keyValueCollections);
            return true;
        }

        #endregion

        #region Try Repeat As

        /// <summary>
        /// Try to repeat as  <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="result"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <returns></returns>
        public static bool TryRepeatAs<TObj>(this IObjectVisitor visitor, out TObj result)
        {
            result = default;
            var ret = visitor.TryRepeat(out var val);
            if (!ret) return false;

            try
            {
                result = (TObj)val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try to repeat as  <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <returns></returns>
        public static bool TryRepeatAs<TObj>(this IObjectVisitor visitor, object instance, out TObj result)
        {
            result = default;
            var ret = visitor.TryRepeat(instance, out var val);
            if (!ret) return false;

            try
            {
                result = (TObj)val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try to repeat as  <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="keyValueCollections"></param>
        /// <param name="result"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <returns></returns>
        public static bool TryRepeatAs<TObj>(this IObjectVisitor visitor, IDictionary<string, object> keyValueCollections, out TObj result)
        {
            result = default;
            var ret = visitor.TryRepeat(keyValueCollections, out var val);
            if (!ret) return false;

            try
            {
                result = (TObj)val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try to repeat as  <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryRepeatAs<T>(this IObjectVisitor<T> visitor, out T result)
        {
            result = default;
            return visitor.TryRepeat(out result);
        }

        /// <summary>
        /// Try to repeat as  <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryRepeatAs<T>(this IObjectVisitor<T> visitor, T instance, out T result)
        {
            result = default;
            return visitor.TryRepeat(instance, out result);
        }

        /// <summary>
        /// Try to repeat as  <br />
        /// 尝试重复
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="keyValueCollections"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryRepeatAs<T>(this IObjectVisitor<T> visitor, IDictionary<string, object> keyValueCollections, out T result)
        {
            result = default;
            return visitor.TryRepeat(keyValueCollections, out result);
        }

        #endregion
    }
}