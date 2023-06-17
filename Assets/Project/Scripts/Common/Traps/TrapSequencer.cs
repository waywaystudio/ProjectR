using System;
using UnityEngine;

namespace Common.Traps
{
    [Serializable]
    public class TrapSequencer : Sequencer<Vector3>
    {
        public ActionTable ExecuteAction { get; } = new();
    }
    
    public class TrapSequenceBuilder : SequenceBuilder<Vector3>
    {
        private TrapSequencer TrapSequencer => Sequencer as TrapSequencer;

        public SequenceBuilder<Vector3> AddExecution(string key, Action action) => Add(SectionType.Execute, key, action);
        public SequenceBuilder<Vector3> AddRelease(string key, Action action) => Add(SectionType.Release, key, action);
        public SequenceBuilder<Vector3> RemoveExecution(string key) => Remove(SectionType.Execute, key);
        public SequenceBuilder<Vector3> RemoveRelease(string key) => Remove(SectionType.Release, key);

        //Override GetActionTable method
        protected override ActionTable GetActionTable(SectionType type) 
            => type == SectionType.Execute ? TrapSequencer.ExecuteAction : base.GetActionTable(type);
    }

    
    public class TrapSequenceInvoker : SequenceInvoker<Vector3>
    {
        private TrapSequencer TrapSequencer => Sequencer as TrapSequencer;
    
        public void Execute() => TrapSequencer.ExecuteAction.Invoke();
    }
}
