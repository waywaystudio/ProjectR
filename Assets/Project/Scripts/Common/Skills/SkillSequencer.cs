using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class SkillSequencer : Sequencer<Vector3>
    {
        public ActionTable ExecuteAction { get; } = new();
        public ActionTable ReleaseAction { get; } = new();
    }
    
    public class SkillSequenceBuilder : SequenceBuilder<Vector3>
    {
        private SkillSequencer SkillSequencer => Sequencer as SkillSequencer;
    
        public SequenceBuilder<Vector3> AddExecution(string key, Action action) => Add(SectionType.Execute, key, action);
        public SequenceBuilder<Vector3> AddRelease(string key, Action action) => Add(SectionType.Release, key, action);
        public SequenceBuilder<Vector3> RemoveExecution(string key) => Remove(SectionType.Execute, key);
        public SequenceBuilder<Vector3> RemoveRelease(string key) => Remove(SectionType.Release, key);
    
        protected override ActionTable GetActionTable(SectionType type) => type switch
        {
            SectionType.Execute => SkillSequencer.ExecuteAction,
            SectionType.Release => SkillSequencer.ReleaseAction,
            _                   => base.GetActionTable(type),
        };
    }
    
    public class SkillSequenceInvoker : SequenceInvoker<Vector3>
    {
        private SkillSequencer SkillSequencer => Sequencer as SkillSequencer;
    
        public void Execute() => SkillSequencer.ExecuteAction.Invoke();
        public void Release() => SkillSequencer.ReleaseAction.Invoke();
    }
}
