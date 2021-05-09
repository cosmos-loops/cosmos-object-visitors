using System;
using Cosmos.Reflection.ObjectVisitors.Metadata;

namespace Cosmos.Reflection.ObjectVisitors.Internals.Loop
{
    internal class InternalObjectLoopingContext
    {
        private readonly bool _withNumber;
        private readonly bool _withMember;
        private Action<string, object, ObjectMember, int> _action0;
        private Action<string, object, ObjectMember> _action1;
        private Action<string, object> _action2;
        
        public InternalObjectLoopingContext(Action<ObjectLoopContext> loopAct)
        {
            if (loopAct is null)
                _action0 = (s, o, m, i) => { };
            else
                _action0 = (s, o, m, i) => loopAct.Invoke(new ObjectLoopContext(s, o, m, i));

            _withMember = true;
            _withNumber = true;
        }

        public InternalObjectLoopingContext(Action<string, object, ObjectMember> loopAct)
        {
            _action1 = loopAct;
            _withMember = true;
            _withNumber = false;
        }

        public InternalObjectLoopingContext(Action<string, object> loopAct)
        {
            _action2 = loopAct;
            _withMember = false;
            _withNumber = false;
        }

        public void Do(string name, object value, ObjectMember member, int? index = null)
        {
            if (_withNumber && index.HasValue)
                _action0?.Invoke(name, value, member, index.Value);
            else if (_withMember)
                _action1?.Invoke(name, value, member);
            else
                _action2?.Invoke(name, value);
        }

        public bool NeedObjectMember => _withMember;

        public bool NeedObjectNumber => _withNumber;
    }
}