using System;
using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors
{
    public class ObjectPropertyValueSetter : IPropertyValueSetter
    {
        internal ObjectPropertyValueSetter() { }
        
        public void Invoke(Type type, object that, string propertyName, object value)
        {
            ObjectSetter.Type(type).Instance(that).SetValue(propertyName, value);
        }

        public void Invoke(Type type, object that, string propertyName, BindingFlags bindingAttr, object value)
        {
            ObjectSetter.Type(type).Instance(that).SetValue(propertyName, value);
        }

        public static IPropertyValueSetter Instance { get; } = new ObjectPropertyValueSetter();
    }
}