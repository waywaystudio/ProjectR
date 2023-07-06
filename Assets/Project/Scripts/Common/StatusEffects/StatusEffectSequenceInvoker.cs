namespace Common.StatusEffects
{
    public class StatusEffectSequenceInvoker : SequenceInvoker
    {
        public StatusEffectSequenceInvoker(Sequencer sequencer) : base(sequencer) { }

        public void Override() => Sequencer[Section.Override]?.Invoke();
    }
}