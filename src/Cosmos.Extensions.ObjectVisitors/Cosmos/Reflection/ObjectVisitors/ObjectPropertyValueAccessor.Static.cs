using System;
using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors
{
    internal class StaticPropertyValueAccessor : IPropertyValueAccessor
    {
        public StaticPropertyValueAccessor(Type type)
        {
            _type = type;
        }

        private readonly Type _type;

        public object GetValue(string propertyName)
        {
            return ObjectGetter.Type(_type).Instance(null).GetValue(propertyName);
        }

        public object GetValue(string propertyName, BindingFlags bindingAttr)
        {
            return ObjectGetter.Type(_type).Instance(null).GetValue(propertyName);
        }

        public void SetValue(string propertyName, object value)
        {
            ObjectSetter.Type(_type).Instance(null).SetValue(propertyName, value);
        }

        public void SetValue(string propertyName, BindingFlags bindingAttr, object value)
        {
            ObjectSetter.Type(_type).Instance(null).SetValue(propertyName, value);
        }
    }

    internal class StaticPropertyValueAccessor<T> : IPropertyValueAccessor
    {
        private readonly Type _type = typeof(T);

        public object GetValue(string propertyName)
        {
            return ObjectGetter.Type(_type).Instance(null).GetValue(propertyName);
        }

        public object GetValue(string propertyName, BindingFlags bindingAttr)
        {
            return ObjectGetter.Type(_type).Instance(null).GetValue(propertyName);
        }

        public void SetValue(string propertyName, object value)
        {
            ObjectSetter.Type(_type).Instance(null).SetValue(propertyName, value);
        }

        public void SetValue(string propertyName, BindingFlags bindingAttr, object value)
        {
            ObjectSetter.Type(_type).Instance(null).SetValue(propertyName, value);
        }
    }
}