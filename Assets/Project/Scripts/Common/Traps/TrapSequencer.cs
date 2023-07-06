using UnityEngine;

namespace Common.Traps
{
    public class TrapSequenceInvoker : SequenceInvoker<Vector3>
    {
        public TrapSequenceInvoker(Sequencer<Vector3> sequencer) : base(sequencer) { }
        
        public void Execute() => Sequencer[Section.Execute].Invoke();
    }
}
