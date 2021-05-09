using System;
using System.Collections.Generic;
using System.Linq;
using Cosmos.Reflection.ObjectVisitors.Core;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Repeat
{
    internal class HistoricalContext
    {
        public HistoricalContext(Type sourceType, AlgorithmKind kind)
        {
            SourceType = sourceType;
            AlgorithmKind = kind;
        }

        public Type SourceType { get; }

        public AlgorithmKind AlgorithmKind { get; }

        protected Action<ObjectCallerBase> _handleHistory;

        public void RegisterOperation(Action<ObjectCallerBase> change)
        {
            if (change is not null)
            {
                _handleHistory = _handleHistory is null
                    ? change
                    : _handleHistory += change;
            }
        }

        public object Repeat()
        {
            var handler = SafeObjectHandleSwitcher.Switch(AlgorithmKind)(SourceType);

            handler.New();

            _handleHistory?.Invoke(handler);

            return handler.GetInstance();
        }

        public object Repeat(object instance)
        {
            var handler = SafeObjectHandleSwitcher.Switch(AlgorithmKind)(SourceType);

            handler.SetObjInstance(instance);

            _handleHistory?.Invoke(handler);

            return handler.GetInstance();
        }

        public object Repeat(IDictionary<string, object> keyValueCollections)
        {
            var handler = SafeObjectHandleSwitcher.Switch(AlgorithmKind)(SourceType);

            handler.New();

            if (keyValueCollections != null && keyValueCollections.Any())
            {
                foreach (var keyValue in keyValueCollections)
                {
                    handler[keyValue.Key] = keyValue.Value;
                }
            }

            _handleHistory?.Invoke(handler);

            return handler.GetInstance();
        }
    }

    internal class HistoricalContext<TObject> : HistoricalContext
    {
        public HistoricalContext(AlgorithmKind kind) : base(typeof(TObject), kind) { }

        public TObject Repeat(TObject instance)
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<TObject>(AlgorithmKind)().With<TObject>();

            handler.SetInstance(instance);

            _handleHistory?.Invoke(handler);

            return handler.GetInstance();
        }

        public new TObject Repeat()
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<TObject>(AlgorithmKind)().With<TObject>();

            handler.New();

            _handleHistory?.Invoke(handler);

            return handler.GetInstance();
        }

        public new TObject Repeat(IDictionary<string, object> keyValueCollections)
        {
            var handler = UnsafeObjectHandleSwitcher.Switch<TObject>(AlgorithmKind)().With<TObject>();

            handler.New();

            if (keyValueCollections != null && keyValueCollections.Any())
            {
                foreach (var keyValue in keyValueCollections)
                {
                    handler[keyValue.Key] = keyValue.Value;
                }
            }

            _handleHistory?.Invoke(handler);

            return handler.GetInstance();
        }
    }
}