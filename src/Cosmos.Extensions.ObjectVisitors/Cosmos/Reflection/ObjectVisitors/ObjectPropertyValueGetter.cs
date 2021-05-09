using System;
using System.Reflection;

namespace Cosmos.Reflection.ObjectVisitors
{
    public class ObjectPropertyValueGetter : IPropertyValueGetter
    {
        internal ObjectPropertyValueGetter() { }

        public object Invoke(Type type, object that, string propertyName)
        {
            return ObjectGetter.Type(type).Instance(that).GetValue(propertyName);
        }

        public object Invoke(Type type, object that, string propertyName, BindingFlags bindingAttr)
        {
            return ObjectGetter.Type(type).Instance(that).GetValue(propertyName);
        }

        public static IPropertyValueGetter Instance { get; } = new ObjectPropertyValueGetter();
    }
}