using System.Linq.Expressions;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Members;

internal class ValueSetter : IObjectValueSetter
{
    private readonly Action<object> _setter;
    private readonly IObjectVisitor _visitor;

    public ValueSetter(IObjectVisitor visitor, string name)
    {
        _visitor = visitor;
        _setter = val => visitor.SetValue(name, val);
    }

    public void Value(object value)
    {
        _setter.Invoke(value);
    }

    public object HostedInstance => _visitor.Instance;
}

internal class ValueSetter<T> : IObjectValueSetter<T>
{
    private readonly Action<object> _setter;
    private readonly IObjectVisitor<T> _visitor;

    public ValueSetter(IObjectVisitor<T> visitor, string name)
    {
        _visitor = visitor;
        _setter = val => visitor.SetValue(name, val);
    }

    public ValueSetter(IObjectVisitor<T> visitor, Expression<Func<T, object>> expression)
    {
        _visitor = visitor;
        _setter = val => visitor.SetValue(expression, val);
    }

    public void Value(object value)
    {
        _setter.Invoke(value);
    }

    public T HostedInstance => _visitor.Instance;
}

internal class ValueSetter<T, TVal> : IObjectValueSetter<T, TVal>
{
    private readonly Action<TVal> _setter;
    private readonly IObjectVisitor<T> _visitor;

    public ValueSetter(IObjectVisitor<T> visitor, Expression<Func<T, TVal>> expression)
    {
        _visitor = visitor;
        _setter = val => visitor.SetValue(expression, val);
    }

    public void Value(TVal value)
    {
        _setter.Invoke(value);
    }

    public T HostedInstance => _visitor.Instance;
}