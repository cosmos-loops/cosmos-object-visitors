using System;
using System.Linq.Expressions;

namespace CosmosStack.Reflection.ObjectVisitors.Internals.Members
{
    internal static class PropertyValueGetter
    {
        public static TVal Get<T, TVal>(Expression<Func<T, TVal>> expression, T source)
        {
            var func = expression.Compile();
            return func.Invoke(source);
        }
    }
}