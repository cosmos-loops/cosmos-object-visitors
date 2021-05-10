#if NET5_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace Cosmos.Reflection.ObjectVisitors.Core
{
#if NET5_0_OR_GREATER
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