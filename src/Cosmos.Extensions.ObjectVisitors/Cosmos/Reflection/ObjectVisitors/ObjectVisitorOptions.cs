using System;
using Cosmos.Reflection.ObjectVisitors.Internals;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors
{
    public class ObjectVisitorOptions
    {
        public AlgorithmKind AlgorithmKind { get; set; } = AlgorithmKind.Precision;

        public bool LiteMode { get; set; } = LvMode.FULL;

        public bool Repeatable { get; set; } = RpMode.REPEATABLE;

        public bool StrictMode { get; set; } = StMode.NORMALE;

        public bool SilenceIfNotWritable { get; set; } = true;

        public ObjectVisitorOptions With(Action<ObjectVisitorOptions> updateAct)
        {
            updateAct?.Invoke(this);
            return this;
        }

        public ObjectVisitorOptions Clone()
        {
            return new()
            {
                AlgorithmKind = AlgorithmKind,
                LiteMode = LiteMode,
                Repeatable = Repeatable,
                StrictMode = StrictMode,
                SilenceIfNotWritable = SilenceIfNotWritable
            };
        }

        public ObjectVisitorOptions Clone(Action<ObjectVisitorOptions> updateAct)
        {
            return Clone().With(updateAct);
        }

        public static ObjectVisitorOptions Default => new();
    }
}