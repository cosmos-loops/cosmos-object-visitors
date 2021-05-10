using System;
using System.Collections.Generic;
using System.Dynamic;
using Cosmos.Exceptions;

namespace Cosmos.Reflection.ObjectVisitors
{
    public sealed class DynamicInstance : DynamicObject
    {
        private readonly IObjectVisitor _visitor;
        private readonly FutureCreatingBuilder<string, object> _gettingMemberHandler;
        private readonly FutureInvokingBuilder<string, object> _settingMemberHandler;
        private readonly Lazy<IObjectGetter> _lazyObjectGetter;
        private readonly Lazy<IObjectSetter> _lazyObjectSetter;

        internal DynamicInstance(IObjectVisitor visitor)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _gettingMemberHandler = Try.CreateFuture<string, object>(name => _visitor.GetValue(name));
            _settingMemberHandler = Try.Future<string, object>((name, val) => _visitor.SetValue(name, val));
            _lazyObjectGetter = new Lazy<IObjectGetter>(() => ObjectGetter.Type(_visitor.SourceType, _visitor.AlgorithmKind).Instance(_visitor.Instance));
            _lazyObjectSetter = new Lazy<IObjectSetter>(() => ObjectSetter.Type(_visitor.SourceType, _visitor.AlgorithmKind).Instance(_visitor.Instance));
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var @try = _gettingMemberHandler.Invoke(binder.Name);
            result = @try.GetSafeValue((object) default);
            return @try.IsSuccess;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var @try = _settingMemberHandler.Invoke(binder.Name, value);
            return @try.IsSuccess;
        }

        public override IEnumerable<string> GetDynamicMemberNames() => _visitor.GetMemberNames();

        public IObjectVisitor ExposeVisitor() => _visitor;

        public IPropertyValueAccessor ExposeValueAccessor() => _visitor.ToValueAccessor();

        public IObjectGetter ExposeGetter() => _lazyObjectGetter.Value;

        public IObjectSetter ExposeSetter() => _lazyObjectSetter.Value;
    }
}