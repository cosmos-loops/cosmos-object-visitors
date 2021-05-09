using System.Collections.Generic;

namespace Cosmos.Reflection.ObjectVisitors
{
    public interface IObjectSelector<TVal>
    {
        IObjectVisitor BackToVisitor();

        IEnumerable<TVal> FireAndReturn();

        IObjectVisitor FireAndBack(out IEnumerable<TVal> returnedVal);
    }

    public interface IObjectSelector<T, TVal>
    {
        IObjectVisitor<T> BackToVisitor();

        IEnumerable<TVal> FireAndReturn();

        IObjectVisitor<T> FireAndBack(out IEnumerable<TVal> returnedVal);
    }
}