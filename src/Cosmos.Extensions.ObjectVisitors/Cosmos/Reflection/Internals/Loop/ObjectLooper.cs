using System;
using Cosmos.Reflection.Internals.Members;
using Cosmos.Reflection.Metadata;

namespace Cosmos.Reflection.Internals.Loop
{
    internal class ObjectLooper : IObjectLooper
    {
        private readonly IObjectVisitor _visitor;
        private readonly Lazy<MemberHandler> _memberHandler;
        private readonly InternalObjectLoopingContext _context;

        public ObjectLooper(IObjectVisitor visitor, Lazy<MemberHandler> memberHandler, Action<string, object, ObjectMember> loopAct)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectLoopingContext(loopAct);
        }

        public ObjectLooper(IObjectVisitor visitor, Lazy<MemberHandler> memberHandler, Action<string, object> loopAct)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectLoopingContext(loopAct);
        }

        public ObjectLooper(IObjectVisitor visitor, Lazy<MemberHandler> memberHandler, Action<ObjectLoopContext> loopAct)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectLoopingContext(loopAct);
        }

        public IObjectVisitor BackToVisitor() => _visitor;

        public void Fire()
        {
            var needObjectMember = _context.NeedObjectMember;
            var needObjectIndex = _context.NeedObjectNumber;
            var index = 0;
            foreach (var name in _visitor.GetMemberNames())
            {
                if (needObjectMember && needObjectIndex)
                    _context.Do(name, _visitor[name], _memberHandler.Value[name], index++);
                else if (needObjectMember)
                    _context.Do(name, _visitor[name], _memberHandler.Value[name]);
                else
                    _context.Do(name, _visitor[name], null);
            }
        }

        public IObjectVisitor FireAndBack()
        {
            Fire();
            return BackToVisitor();
        }
    }

    internal class ObjectLooper<T> : IObjectLooper<T>
    {
        private readonly IObjectVisitor<T> _visitor;
        private readonly Lazy<MemberHandler> _memberHandler;
        private readonly InternalObjectLoopingContext _context;

        public ObjectLooper(IObjectVisitor<T> visitor, Lazy<MemberHandler> memberHandler, Action<string, object, ObjectMember> loopAct)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectLoopingContext(loopAct);
        }

        public ObjectLooper(IObjectVisitor<T> visitor, Lazy<MemberHandler> memberHandler, Action<string, object> loopAct)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectLoopingContext(loopAct);
        }

        public ObjectLooper(IObjectVisitor<T> visitor, Lazy<MemberHandler> memberHandler, Action<ObjectLoopContext> loopAct)
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            _memberHandler = memberHandler ?? throw new ArgumentNullException(nameof(memberHandler));
            _context = new InternalObjectLoopingContext(loopAct);
        }

        public IObjectVisitor<T> BackToVisitor() => _visitor;

        public void Fire()
        {
            var needObjectMember = _context.NeedObjectMember;
            var needObjectIndex = _context.NeedObjectNumber;
            var index = 0;
            foreach (var name in _visitor.GetMemberNames())
            {
                if (needObjectMember && needObjectIndex)
                    _context.Do(name, _visitor[name], _memberHandler.Value[name], index++);
                else if (needObjectMember)
                    _context.Do(name, _visitor[name], _memberHandler.Value[name]);
                else
                    _context.Do(name, _visitor[name], null);
            }
        }

        public IObjectVisitor<T> FireAndBack()
        {
            Fire();
            return BackToVisitor();
        }
    }
}