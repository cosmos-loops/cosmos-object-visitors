using System;
using System.Linq.Expressions;

namespace Cosmos.Reflection.Internals.Members
{
    internal class ValueGetter : IObjectValueGetter
    {
        private readonly Func<object> _getter;
        private readonly IObjectVisitor _visitor;

        public ValueGetter(IObjectVisitor visitor, string name)
        {
            _visitor = visitor;
            _getter = () => visitor.GetValue(name);
        }

        public object Value => _getter.Invoke();

        public object HostedInstance => _visitor.Instance;
    }

    internal class ValueGetter<T> : IObjectValueGetter<T>
    {
        private readonly Func<object> _getter;
        private readonly IObjectVisitor<T> _visitor;
        
        public ValueGetter(IObjectVisitor<T> visitor, string name)
        {
            _visitor = visitor;
            _getter = () => visitor.GetValue(name);
        }

        public ValueGetter(IObjectVisitor<T> visitor, Expression<Func<T, object>> expression)
        {
            _visitor = visitor;
            _getter = () => visitor.GetValue(expression);
        }

        public object Value => _getter.Invoke();

        public T HostedInstance => _visitor.Instance;
    }

    internal class ValueGetter<T, TVal> : IObjectValueGetter<T, TVal>
    {
        private readonly Func<TVal> _getter;
        private readonly IObjectVisitor<T> _visitor;

        public ValueGetter(IObjectVisitor<T> visitor, Expression<Func<T, TVal>> expression)
        {
            _visitor = visitor;
            _getter = () => visitor.GetValue(expression);
        }

        public TVal Value => _getter.Invoke();

        public T HostedInstance => _visitor.Instance;
    }
}