namespace Cosmos.Reflection
{
    public interface IObjectValueSetter
    {
        void Value(object value);

        object HostedInstance { get; }
    }

    public interface IObjectValueSetter<out T>
    {
        void Value(object value);

        T HostedInstance { get; }
    }

    public interface IObjectValueSetter<out T, in TVal>
    {
        void Value(TVal value);

        T HostedInstance { get; }
    }
}