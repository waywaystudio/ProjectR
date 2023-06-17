using System;

namespace Common.StatusEffect
{
    [Serializable]
    public class StatusEffectSequencer : Sequencer<ICombatTaker>
    {
        public ActionTable ExecuteAction { get; } = new();
        public ActionTable OverrideAction { get; } = new();
    }
    
    public class StatusEffectSequenceBuilder : SequenceBuilder<ICombatTaker>
    {
        private StatusEffectSequencer StatusEffectSequencer => Sequencer as StatusEffectSequencer;
    
        public SequenceBuilder<ICombatTaker> AddExecution(string key, Action action) => Add(SectionType.Execute, key, action);
        public SequenceBuilder<ICombatTaker> AddOverride(string key, Action action) => Add(SectionType.Release, key, action);
        public SequenceBuilder<ICombatTaker> RemoveExecution(string key) => Remove(SectionType.Execute, key);
        public SequenceBuilder<ICombatTaker> RemoveOverride(string key) => Remove(SectionType.Release, key);
    
        protected override ActionTable GetActionTable(SectionType type) => type switch
        {
            SectionType.Execute => StatusEffectSequencer.ExecuteAction,
            SectionType.Override => StatusEffectSequencer.OverrideAction,
            _                   => base.GetActionTable(type),
        };
    }
    
    public class StatusEffectSequenceInvoker : SequenceInvoker<ICombatTaker>
    {
        private StatusEffectSequencer SkillSequencer => Sequencer as StatusEffectSequencer;
    
        public void Execute() => SkillSequencer.ExecuteAction.Invoke();
        public void Overriding() => SkillSequencer.OverrideAction.Invoke();
    }
    

}