using System.Collections.Generic;

namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectRepeater
    {
        object Play(object originalObj);

        object Play(IDictionary<string, object> keyValueCollections);

        object NewAndPlay();
    }

    public interface IObjectRepeater<T> : IObjectRepeater
    {
        T Play(T originalObj);

        new T Play(IDictionary<string, object> keyValueCollections);

        new T NewAndPlay();
    }
}