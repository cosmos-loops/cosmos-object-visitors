using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Internals.Loop;
using Cosmos.Reflection.ObjectVisitors.Internals.Repeat;
using Cosmos.Reflection.ObjectVisitors.Internals.Select;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors
{
    public static class ObjectVisitorExtensions
    {
        #region To Visitor

        public static IObjectVisitor<T> ToVisitor<T>(this T instanceObj,
            AlgorithmKind kind = AlgorithmKind.Precision,
            bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE)
            where T : class
        {
            return ObjectVisitorFactory.Create(instanceObj, kind, repeatable, strictMode);
        }

        public static IObjectVisitor ToVisitor(this Type type,
            AlgorithmKind kind = AlgorithmKind.Precision,
            bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE)
        {
            return ObjectVisitorFactory.Create(type, kind, repeatable, strictMode);
        }

        public static IObjectVisitor ToVisitor(this Type type, IDictionary<string, object> initialValues,
            AlgorithmKind kind = AlgorithmKind.Precision,
            bool repeatable = RpMode.REPEATABLE,
            bool strictMode = StMode.NORMALE)
        {
            return ObjectVisitorFactory.Create(type, initialValues, kind, repeatable, strictMode);
        }

        #endregion

        #region ToDynamicObject

        public static DynamicInstance ToDynamicObject(this IObjectVisitor visitor)
        {
            return new(visitor);
        }

        #endregion

        #region To Dictionary

        public static Dictionary<string, object> ToDictionary(this IObjectVisitor visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var val = new Dictionary<string, object>();
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            var lazyHandler = rel.ExposeLazyMemberHandler();
            foreach (var name in lazyHandler.Value.GetNames())
                val[name] = visitor.GetValue(name);
            return val;
        }

        public static Dictionary<string, object> ToDictionary<T>(this IObjectVisitor<T> visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var val = new Dictionary<string, object>();
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            var lazyHandler = rel.ExposeLazyMemberHandler();
            foreach (var name in lazyHandler.Value.GetNames())
                val[name] = visitor.GetValue(name);
            return val;
        }

        #endregion

        #region Select

        public static IObjectSelector<TVal> Select<TVal>(this IObjectVisitor visitor, Func<string, object, ObjectMember, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        public static IObjectSelector<TVal> Select<TVal>(this IObjectVisitor visitor, Func<string, object, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        public static IObjectSelector<TVal> Select<TVal>(this IObjectVisitor visitor, Func<ObjectLoopContext, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        public static IObjectSelector<T, TVal> Select<T, TVal>(this IObjectVisitor<T> visitor, Func<string, object, ObjectMember, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<T, TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        public static IObjectSelector<T, TVal> Select<T, TVal>(this IObjectVisitor<T> visitor, Func<string, object, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<T, TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        public static IObjectSelector<T, TVal> Select<T, TVal>(this IObjectVisitor<T> visitor, Func<ObjectLoopContext, TVal> loopFunc)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectSelector<T, TVal>(rel.Owner, rel.ExposeLazyMemberHandler(), loopFunc);
        }

        #endregion

        #region For Each

        public static IObjectLooper ForEach(this IObjectVisitor visitor, Action<string, object, ObjectMember> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        public static IObjectLooper ForEach(this IObjectVisitor visitor, Action<string, object> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        public static IObjectLooper ForEach(this IObjectVisitor visitor, Action<ObjectLoopContext> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        public static IObjectLooper<T> ForEach<T>(this IObjectVisitor<T> visitor, Action<string, object, ObjectMember> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper<T>(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        public static IObjectLooper<T> ForEach<T>(this IObjectVisitor<T> visitor, Action<string, object> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper<T>(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        public static IObjectLooper<T> ForEach<T>(this IObjectVisitor<T> visitor, Action<ObjectLoopContext> loopAct)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.LiteMode == LvMode.LITE)
                throw new InvalidOperationException("Lite mode visitor has no Member handler.");
            return new ObjectLooper<T>(rel.Owner, rel.ExposeLazyMemberHandler(), loopAct);
        }

        #endregion

        #region For Repeat

        public static IObjectRepeater ForRepeat(this IObjectVisitor visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            if (visitor.IsStatic) return new EmptyRepeater(visitor.SourceType);
            var rel = (ICoreVisitor) visitor;
            if (rel.ExposeHistoricalContext() is null) return new EmptyRepeater(visitor.SourceType);
            return new ObjectRepeater(rel.ExposeHistoricalContext());
        }

        public static IObjectRepeater<T> ForRepeat<T>(this IObjectVisitor<T> visitor)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            if (visitor.IsStatic) return new EmptyRepeater<T>();
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.ExposeHistoricalContext() is null) return new EmptyRepeater<T>();
            return new ObjectRepeater<T>(rel.ExposeHistoricalContext());
        }

        #endregion

        #region Try Repeat

        public static bool TryRepeat(this IObjectVisitor visitor, out object result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor) visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat();
            return true;
        }

        public static bool TryRepeat(this IObjectVisitor visitor, object instance, out object result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor) visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(instance);
            return true;
        }

        public static bool TryRepeat(this IObjectVisitor visitor, IDictionary<string, object> keyValueCollections, out object result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor) visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(keyValueCollections);
            return true;
        }

        public static bool TryRepeat<T>(this IObjectVisitor<T> visitor, out T result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat();
            return true;
        }

        public static bool TryRepeat<T>(this IObjectVisitor<T> visitor, T instance, out T result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(instance);
            return true;
        }

        public static bool TryRepeat<T>(this IObjectVisitor<T> visitor, IDictionary<string, object> keyValueCollections, out T result)
        {
            if (visitor is null)
                throw new ArgumentNullException(nameof(visitor));
            result = default;
            if (visitor.IsStatic) return false;
            var rel = (ICoreVisitor<T>) visitor;
            if (rel.ExposeHistoricalContext() is null) return false;
            result = rel.ExposeHistoricalContext().Repeat(keyValueCollections);
            return true;
        }

        #endregion

        #region Try Repeat As

        public static bool TryRepeatAs<TObj>(this IObjectVisitor visitor, out TObj result)
        {
            result = default;
            var ret = visitor.TryRepeat(out var val);
            if (!ret) return false;

            try
            {
                result = (TObj) val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryRepeatAs<TObj>(this IObjectVisitor visitor, object instance, out TObj result)
        {
            result = default;
            var ret = visitor.TryRepeat(instance, out var val);
            if (!ret) return false;

            try
            {
                result = (TObj) val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryRepeatAs<TObj>(this IObjectVisitor visitor, IDictionary<string, object> keyValueCollections, out TObj result)
        {
            result = default;
            var ret = visitor.TryRepeat(keyValueCollections, out var val);
            if (!ret) return false;

            try
            {
                result = (TObj) val;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryRepeatAs<T>(this IObjectVisitor<T> visitor, out T result)
        {
            result = default;
            return visitor.TryRepeat(out result);
        }

        public static bool TryRepeatAs<T>(this IObjectVisitor<T> visitor, T instance, out T result)
        {
            result = default;
            return visitor.TryRepeat(instance, out result);
        }

        public static bool TryRepeatAs<T>(this IObjectVisitor<T> visitor, IDictionary<string, object> keyValueCollections, out T result)
        {
            result = default;
            return visitor.TryRepeat(keyValueCollections, out result);
        }

        #endregion
    }
}