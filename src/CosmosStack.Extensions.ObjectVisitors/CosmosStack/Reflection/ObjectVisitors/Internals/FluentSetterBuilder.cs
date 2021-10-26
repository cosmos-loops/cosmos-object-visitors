using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CosmosStack.Reflection.ObjectVisitors.Internals.Members;

namespace CosmosStack.Reflection.ObjectVisitors.Internals
{
    internal class FluentSetterBuilder : IFluentSetter, IFluentValueSetter
    {
        private readonly Type _type;
        private readonly ObjectVisitorOptions _options;

        public FluentSetterBuilder(Type type, ObjectVisitorOptions options)
        {
            _type = type;
            _options = options;
        }

        #region Fluent building methods for Instance Setter

        IObjectSetter IFluentSetter.NewInstance(bool strictMode)
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.StrictMode = strictMode);
            if (_type.IsAbstract && _type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(_type, clone);
            return ObjectVisitorFactoryCore.CreateForFutureInstance(_type, clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE));
        }

        IObjectSetter IFluentSetter.Instance(object instance, bool strictMode)
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.StrictMode = strictMode);
            if (_type.IsAbstract && _type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType(_type, clone);
            return ObjectVisitorFactoryCore.CreateForInstance(_type, instance, clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE));
        }

        IObjectSetter IFluentSetter.InitialValues(IDictionary<string, object> initialValues, bool strictMode)
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.StrictMode = strictMode);
            if (_type.IsAbstract && _type.IsSealed)
            {
                var visitor = ObjectVisitorFactoryCore.CreateForStaticType(_type, clone);
                visitor.SetValue(initialValues);
                return visitor;
            }

            return ObjectVisitorFactoryCore.CreateForFutureInstance(_type, clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE), initialValues);
        }

        #endregion

        #region Fluent building methods for Value Setter

        private Func<object, bool, ValueSetter> _func1;

        IFluentValueSetter IFluentSetter.Value(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance(_type, t, clone);
                return new ValueSetter(visitor, propertyInfo.Name);
            };

            return this;
        }

        IFluentValueSetter IFluentSetter.Value(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance(_type, t, clone);
                return new ValueSetter(visitor, fieldInfo.Name);
            };

            return this;
        }

        IFluentValueSetter IFluentSetter.Value(string memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentNullException(nameof(memberName));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance(_type, t, clone);
                return new ValueSetter(visitor, memberName);
            };

            return this;
        }


        IObjectValueSetter IFluentValueSetter.Instance(object instance, bool strictMode)
        {
            return _func1.Invoke(instance, strictMode);
        }

        #endregion
    }

    internal class FluentSetterBuilder<T> : IFluentSetter<T>, IFluentValueSetter<T>
    {
        private readonly Type _type;
        private readonly ObjectVisitorOptions _options;

        public FluentSetterBuilder(ObjectVisitorOptions options)
        {
            _type = typeof(T);
            _options = options;
        }

        #region Fluent building methods for Instance Setter

        IObjectSetter<T> IFluentSetter<T>.NewInstance(bool strictMode)
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.StrictMode = strictMode);
            if (_type.IsAbstract && _type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(clone);
            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE));
        }

        IObjectSetter<T> IFluentSetter<T>.Instance(T instance, bool strictMode)
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.StrictMode = strictMode);
            if (_type.IsAbstract && _type.IsSealed)
                return ObjectVisitorFactoryCore.CreateForStaticType<T>(clone);
            return ObjectVisitorFactoryCore.CreateForInstance<T>(instance, clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE));
        }

        IObjectSetter<T> IFluentSetter<T>.InitialValues(IDictionary<string, object> initialValues, bool strictMode)
        {
            var clone = _options
                        .Clone(x => x.LiteMode = LvMode.LITE)
                        .With(x => x.StrictMode = strictMode);
            if (_type.IsAbstract && _type.IsSealed)
            {
                var visitor = ObjectVisitorFactoryCore.CreateForStaticType<T>(clone);
                visitor.SetValue(initialValues);
                return visitor;
            }

            return ObjectVisitorFactoryCore.CreateForFutureInstance<T>(clone.With(x => x.Repeatable = RpMode.NON_REPEATABLE), initialValues);
        }

        #endregion

        #region Fluent building methods for Value Setter

        private Func<T, bool, ValueSetter<T>> _func1;

        IFluentValueSetter<T> IFluentSetter<T>.Value(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
                return new ValueSetter<T>(visitor, propertyInfo.Name);
            };

            return this;
        }

        IFluentValueSetter<T> IFluentSetter<T>.Value(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
                return new ValueSetter<T>(visitor, fieldInfo.Name);
            };

            return this;
        }

        IFluentValueSetter<T> IFluentSetter<T>.Value(string memberName)
        {
            if (string.IsNullOrWhiteSpace(memberName))
                throw new ArgumentNullException(nameof(memberName));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
                return new ValueSetter<T>(visitor, memberName);
            };

            return this;
        }

        IFluentValueSetter<T> IFluentSetter<T>.Value(Expression<Func<T, object>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            _func1 = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
                return new ValueSetter<T>(visitor, expression);
            };

            return this;
        }

        IFluentValueSetter<T, TVal> IFluentSetter<T>.Value<TVal>(Expression<Func<T, TVal>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            Func<T, bool, ValueSetter<T, TVal>> func = (t, strictMode) =>
            {
                var clone = _options
                            .Clone(x => x.LiteMode = LvMode.LITE)
                            .With(x => x.Repeatable = RpMode.NON_REPEATABLE)
                            .With(x => x.StrictMode = strictMode);
                var visitor = ObjectVisitorFactoryCore.CreateForInstance<T>(t, clone);
                return new ValueSetter<T, TVal>(visitor, expression);
            };

            return new FluentSetterBuilder<T, TVal>(func);
        }

        IObjectValueSetter<T> IFluentValueSetter<T>.Instance(T instance, bool strictMode)
        {
            return _func1.Invoke(instance, strictMode);
        }

        #endregion
    }

    internal class FluentSetterBuilder<T, TVal> : IFluentValueSetter<T, TVal>
    {
        public FluentSetterBuilder(Func<T, bool, ValueSetter<T, TVal>> func)
        {
            _func1 = func;
        }

        #region Fluent building methods for Value Setter

        private Func<T, bool, ValueSetter<T, TVal>> _func1;

        IObjectValueSetter<T, TVal> IFluentValueSetter<T, TVal>.Instance(T instance, bool strictMode)
        {
            return _func1.Invoke(instance, strictMode);
        }

        #endregion
    }
}