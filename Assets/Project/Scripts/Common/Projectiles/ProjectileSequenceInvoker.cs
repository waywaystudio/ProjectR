namespace Common.Projectiles
{
    public class ProjectileSequenceInvoker : SequenceInvoker
    {
        public ProjectileSequenceInvoker(Sequencer sequencer) : base(sequencer) { }

        public void Execute() => Sequencer[SectionType.Execute]?.Invoke();
    }
}
