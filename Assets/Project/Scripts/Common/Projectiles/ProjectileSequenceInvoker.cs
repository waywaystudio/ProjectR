namespace Common.Projectiles
{
    public class ProjectileSequenceInvoker : SequenceInvoker
    {
        public ProjectileSequenceInvoker(Sequencer sequencer) : base(sequencer) { }

        public void Execute() => Sequencer[Section.Execute]?.Invoke();
    }
}
