using System;
using System.Collections.Generic;
using Cosmos.Reflection.ObjectVisitors.Internals.Members;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Select
{
    internal class ObjectSelector<TVal> : IObjectSelector<TVal>
    {
        private readonly IObjectVisitor _visitor;
        private readonly Lazy<MemberHandler> _memberHandler;
        private readonly InternalObjectSelectingContext<TVal> _context;

        public ObjectSelector(IObjectVisitor visitor, Lazy<MemberHandler> memberHandler, Func<string, object, ObjectMember, TVal> loopFunc)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectSelectingContext<TVal>(loopFunc);
        }

        public ObjectSelector(IObjectVisitor visitor, Lazy<MemberHandler> memberHandler, Func<string, object, TVal> loopFunc)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectSelectingContext<TVal>(loopFunc);
        }

        public ObjectSelector(IObjectVisitor visitor, Lazy<MemberHandler> memberHandler, Func<ObjectLoopContext, TVal> loopFunc)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectSelectingContext<TVal>(loopFunc);
        }

        public IObjectVisitor BackToVisitor() => _visitor;

        public IEnumerable<TVal> FireAndReturn()
        {
            var needObjectMember = _context.NeedObjectMember;
            var needObjectIndex = _context.NeedObjectNumber;
            var index = 0;
            foreach (var name in _visitor.GetMemberNames())
            {
                if (needObjectMember && needObjectIndex)
                    yield return _context.Do(name, _visitor[name], _memberHandler.Value[name], index++);
                else if (needObjectMember)
                    yield return _context.Do(name, _visitor[name], _memberHandler.Value[name]);
                else
                    yield return _context.Do(name, _visitor[name], null);
            }
        }

        public IObjectVisitor FireAndBack(out IEnumerable<TVal> returnedVal)
        {
            returnedVal = FireAndReturn();
            return BackToVisitor();
        }
    }

    internal class ObjectSelector<T, TVal> : IObjectSelector<T, TVal>
    {
        private readonly IObjectVisitor<T> _visitor;
        private readonly Lazy<MemberHandler> _memberHandler;
        private readonly InternalObjectSelectingContext<TVal> _context;

        public ObjectSelector(IObjectVisitor<T> visitor, Lazy<MemberHandler> memberHandler, Func<string, object, ObjectMember, TVal> loopFunc)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectSelectingContext<TVal>(loopFunc);
        }

        public ObjectSelector(IObjectVisitor<T> visitor, Lazy<MemberHandler> memberHandler, Func<string, object, TVal> loopFunc)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectSelectingContext<TVal>(loopFunc);
        }

        public ObjectSelector(IObjectVisitor<T> visitor, Lazy<MemberHandler> memberHandler, Func<ObjectLoopContext, TVal> loopFunc)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectSelectingContext<TVal>(loopFunc);
        }

        public IObjectVisitor<T> BackToVisitor() => _visitor;

        public IEnumerable<TVal> FireAndReturn()
        {
            var needObjectMember = _context.NeedObjectMember;
            var needObjectIndex = _context.NeedObjectNumber;
            var index = 0;
            foreach (var name in _visitor.GetMemberNames())
            {
                if (needObjectMember && needObjectIndex)
                    yield return _context.Do(name, _visitor[name], _memberHandler.Value[name], index++);
                else if (needObjectMember)
                    yield return _context.Do(name, _visitor[name], _memberHandler.Value[name]);
                else
                    yield return _context.Do(name, _visitor[name], null);
            }
        }

        public IObjectVisitor<T> FireAndBack(out IEnumerable<TVal> returnedVal)
        {
            returnedVal = FireAndReturn();
            return BackToVisitor();
        }
    }
}