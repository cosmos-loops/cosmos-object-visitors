#if !NETFRAMEWORK

using System;
using System.Collections.Concurrent;
using System.Linq;
using BTFindTree;
using Cosmos.Reflection.Metadata;
using Natasha.CSharp;

namespace Cosmos.Reflection.Core.Builder
{
    public static class PrecisionDictBuilder
    {
        static PrecisionDictBuilder()
        {
            _type_cache = new ConcurrentDictionary<Type, string>();
            _str_cache = new ConcurrentDictionary<string, string>();
        }

        private static readonly ConcurrentDictionary<Type, string> _type_cache;
        private static readonly ConcurrentDictionary<string, string> _str_cache;

        public static unsafe ObjectCallerBase Ctor(Type type)
        {
            //获得动态生成的类型
            var proxyType = ObjectCallerBuilder.InitType(type, AlgorithmKind.Fuzzy);

            //加入缓存
            var script = $"return new {proxyType.GetDevelopName()}();";
            _str_cache[type.GetDevelopName()] = script;
            _type_cache[type] = script;

            var newFindTree = "var str = arg.GetDevelopName();";
            newFindTree += BTFTemplate.GetGroupPrecisionPointBTFScript(_str_cache, "str");
            newFindTree += $"return PrecisionDictBuilder.Ctor(arg);";


            //生成脚本
            var newAction = NDelegate
                            .UseDomain(type.GetDomain(), builder => builder.LogCompilerError())
                            .UnsafeFunc<Type, ObjectCallerBase>(newFindTree, _type_cache.Keys.ToArray(), "Cosmos.Reflection.NCallerDynamic");

            PrecisionDictOperator.CreateFromString = (delegate * managed<Type, ObjectCallerBase>) (newAction.Method.MethodHandle.GetFunctionPointer());
            return (ObjectCallerBase) Activator.CreateInstance(proxyType);
        }
    }

    public static unsafe class PrecisionDictOperator
    {
        public static delegate* managed<Type, ObjectCallerBase> CreateFromString;

        static PrecisionDictOperator()
        {
            PrecisionDictBuilder.Ctor(typeof(NullObjectClass));
        }

        public static ObjectCallerBase CreateFromType(Type type)
        {
            return CreateFromString(type);
        }
    }

    public static unsafe class PrecisionDictOperator<T>
    {
        public static readonly delegate* managed<ObjectCallerBase> Create;

        static PrecisionDictOperator()
        {
            var dynamicType = ObjectCallerBuilder.InitType(typeof(T), AlgorithmKind.Precision);
            Create = (delegate * managed<ObjectCallerBase>) (NInstance.Creator(dynamicType).Method.MethodHandle.GetFunctionPointer());
        }
    }
}

#endif