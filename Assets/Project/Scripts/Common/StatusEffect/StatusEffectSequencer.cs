namespace Common.StatusEffect
{
    public class StatusEffectSequenceInvoker : SequenceInvoker<ICombatTaker>
    {
        public StatusEffectSequenceInvoker(Sequencer<ICombatTaker> sequencer) => Sequencer = sequencer;
        
        public void Execute() => Sequencer[SectionType.Execute].Invoke();
        public void Overriding() => Sequencer[SectionType.Override].Invoke();
    }
}