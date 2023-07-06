using System;
using Common;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Villains
{
    [Serializable]
    public class PhaseSequencer
    {
        [SerializeField] private Sequencer sequencer;
        [SerializeField] private UnityEvent persistantActiveEvent;
        [SerializeField] private UnityEvent persistantCompleteEvent;
        [SerializeField] private VillainPhaseMask phaseMask;

        public SequenceBuilder SequenceBuilder { get; private set; }
        public SequenceInvoker SequenceInvoker { get; private set; }

        public VillainPhaseMask PhaseMask => phaseMask;

        public void Initialize()
        {
            SequenceInvoker = new SequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder(sequencer);

            if (persistantActiveEvent.GetPersistentEventCount() != 0) 
                SequenceBuilder.Add(Section.Active,"PersistantEvent", persistantActiveEvent.Invoke);
            
            if (persistantCompleteEvent.GetPersistentEventCount() != 0)
                SequenceBuilder.Add(Section.Complete,"PersistantEvent", persistantCompleteEvent.Invoke);
        }

        public void Clear() => sequencer.Clear();
    }
}
