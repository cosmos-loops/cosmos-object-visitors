#if NET5_0
using System.Runtime.CompilerServices;
#endif

namespace Cosmos.Reflection.ObjectVisitors.Core
{
#if NET5_0
    [SkipLocalsInit]
#endif
    public abstract class ObjectCallerBase<T> : ObjectCallerBase
    {
        public T Instance;

        public void SetInstance(T value) => Instance = value;

        public override void SetObjInstance(object obj)
        {
            Instance = (T) obj;
        }
    }
}