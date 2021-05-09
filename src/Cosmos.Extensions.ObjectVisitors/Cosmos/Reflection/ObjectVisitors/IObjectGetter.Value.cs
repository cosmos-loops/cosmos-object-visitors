namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectValueGetter
    {
        object Value { get; }

        object HostedInstance { get; }
    }

    public interface IObjectValueGetter<out T>
    {
        object Value { get; }

        T HostedInstance { get; }
    }

    public interface IObjectValueGetter<out T, out TVal>
    {
        TVal Value { get; }

        T HostedInstance { get; }
    }
}