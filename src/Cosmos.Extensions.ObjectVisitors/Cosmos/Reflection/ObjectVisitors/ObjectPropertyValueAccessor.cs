using System;
using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors
{
    public class ObjectPropertyValueAccessor : IPropertyValueAccessor
    {
        internal ObjectPropertyValueAccessor(object instance, Type type)
        {
            _instance = instance;
            _type = type;
        }

        private readonly Type _type;
        private readonly object _instance;

        public object GetValue(string propertyName)
        {
            return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
        }

        public object GetValue(string propertyName, BindingFlags bindingAttr)
        {
            return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
        }

        public void SetValue(string propertyName, object value)
        {
            ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
        }

        public void SetValue(string propertyName, BindingFlags bindingAttr, object value)
        {
            ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
        }

        public static IPropertyValueAccessor Of<T>(T instance) => new ObjectPropertyValueAccessor<T>(instance);

        public static IPropertyValueAccessor Of(object instance, Type type) => new ObjectPropertyValueAccessor(instance, type);

        public static IPropertyValueAccessor OfStatic<T>() => new StaticPropertyValueAccessor<T>();

        public static IPropertyValueAccessor OfStatic(Type type) => new StaticPropertyValueAccessor(type);
    }

    public class ObjectPropertyValueAccessor<T> : IPropertyValueAccessor
    {
        internal ObjectPropertyValueAccessor(object instance)
        {
            _instance = instance;
        }

        private readonly Type _type = typeof(T);
        private readonly object _instance;

        public object GetValue(string propertyName)
        {
            return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
        }

        public object GetValue(string propertyName, BindingFlags bindingAttr)
        {
            return ObjectGetter.Type(_type).Instance(_instance).GetValue(propertyName);
        }

        public void SetValue(string propertyName, object value)
        {
            ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
        }

        public void SetValue(string propertyName, BindingFlags bindingAttr, object value)
        {
            ObjectSetter.Type(_type).Instance(_instance).SetValue(propertyName, value);
        }
    }
}