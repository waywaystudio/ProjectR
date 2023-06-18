using UnityEngine;

namespace Common.Traps
{
    public class TrapSequenceInvoker : SequenceInvoker<Vector3>
    {
        public TrapSequenceInvoker(Sequencer<Vector3> sequencer) => Sequencer = sequencer;
        
        public void Execute() => Sequencer[SectionType.Execute].Invoke();
    }
}
