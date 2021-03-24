using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Reflection.Internals.Members;
using Cosmos.Reflection.Metadata;

namespace Cosmos.Reflection.Internals
{
    internal class FluentGetterBuilder : IFluentGetter, IFluentValueGetter
    {
        private readonly Type _type;
        private readonly AlgorithmKind _kind;

        public FluentGetterBuilder(Type type, AlgorithmKind kind)
        {
            _type = type;
            _kind = kind;
        }

        #region Fluent building methods for Instance Getter

        IObjectGetter IFluentGetter.Instance(object instance)
        {
            if (_type.IsAbstract && _type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(_type, _kind, LvMode.LITE, StMode.NORMALE);
            return ObjectVisitorFactoryCore.CreateForInstance(_type, instance, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
        }

        IObjectGetter IFluentGetter.InitialValues(IDictionary<string, object> initialValues)
        {
            if (_type.IsAbstract && _type.IsSealed)
            {
                var visitor = ObjectVisitorFactoryCore.CreateForStaticType(_type, _kind, LvMode.LITE, StMode.NORMALE);
                visitor.SetValue(initialValues);
                return visitor;
            }

            return ObjectVisitorFactoryCore.CreateForFutureInstance(_type, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE, initialValues);
        }

        #endregion

        #region Fluent building methods for Value Getter

        private Func<object, ValueGetter> _func1;

        IFluentValueGetter IFluentGetter.Value(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance(_type, t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter(visitor, propertyInfo.Name);
            };

            return this;
        }

        IFluentValueGetter IFluentGetter.Value(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance(_type, t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter(visitor, fieldInfo.Name);
            };

            return this;
        }

        IFluentValueGetter IFluentGetter.Value(string memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentNullException(nameof(memberName));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance(_type, t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter(visitor, memberName);
            };

            return this;
        }

        IObjectValueGetter IFluentValueGetter.Instance(object instance)
        {
            return _func1.Invoke(instance);
        }

        #endregion
    }

    internal class FluentGetterBuilder<T> : IFluentGetter<T>, IFluentValueGetter<T>
    {
        private readonly Type _type;
        private readonly AlgorithmKind _kind;

        public FluentGetterBuilder(AlgorithmKind kind)
        {
            _type = typeof(T);
            _kind = kind;
        }

        #region Fluent building methods for Instance Getter

        IObjectGetter<T> IFluentGetter<T>.Instance(T instance)
        {
            if (_type.IsAbstract && _type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(_kind, LvMode.LITE, StMode.NORMALE);
            return ObjectVisitorFactoryCore.CreateForInstance<T>(instance, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
        }

        IObjectGetter<T> IFluentGetter<T>.InitialValues(IDictionary<string, object> initialValues)
        {
            if (_type.IsAbstract && _type.IsSealed)
            {
                var visitor = ObjectVisitorFactoryCore.CreateForStaticType<T>(_kind, LvMode.LITE, StMode.NORMALE);
                visitor.SetValue(initialValues);
                return visitor;
            }

            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(_kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE, initialValues);
        }

        #endregion

        #region Fluent building methods for Value Getter

        private Func<T, ValueGetter<T>> _func1;

        IFluentValueGetter<T> IFluentGetter<T>.Value(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter<T>(visitor, propertyInfo.Name);
            };

            return this;
        }

        IFluentValueGetter<T> IFluentGetter<T>.Value(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter<T>(visitor, fieldInfo.Name);
            };

            return this;
        }

        IFluentValueGetter<T> IFluentGetter<T>.Value(string memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentNullException(nameof(memberName));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter<T>(visitor, memberName);
            };

            return this;
        }

        IFluentValueGetter<T> IFluentGetter<T>.Value(Expression<Func<T, object>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            _func1 = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter<T>(visitor, expression);
            };

            return this;
        }

        IFluentValueGetter<T, TVal> IFluentGetter<T>.Value<TVal>(Expression<Func<T, TVal>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            Func<T, ValueGetter<T, TVal>> func = t =>
            {
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, _kind, RpMode.NON_REPEATABLE, LvMode.LITE, StMode.NORMALE);
                return new ValueGetter<T, TVal>(visitor, expression);
            };

            return new FluentGetterBuilder<T, TVal>(func);
        }

        IObjectValueGetter<T> IFluentValueGetter<T>.Instance(T instance)
        {
            return _func1.Invoke(instance);
        }

        #endregion
    }

    internal class FluentGetterBuilder<T, TVal> : IFluentValueGetter<T, TVal>
    {
        public FluentGetterBuilder(Func<T, ValueGetter<T, TVal>> func)
        {
            _func1 = func;
        }

        #region Fluent building methods for Value Getter

        private Func<T, ValueGetter<T, TVal>> _func1;

        IObjectValueGetter<T, TVal> IFluentValueGetter<T, TVal>.Instance(T instance)
        {
            return _func1.Invoke(instance);
        }

        #endregion
    }
}