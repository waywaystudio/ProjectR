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

        public SequenceBuilder SequenceBuilder { get; } = new();
        public SequenceInvoker SequenceInvoker { get; } = new();

        public VillainPhaseMask PhaseMask => phaseMask;

        public void Initialize()
        {
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer);

            if (persistantActiveEvent.GetPersistentEventCount() != 0) 
                SequenceBuilder.Add(SectionType.Active,"PersistantEvent", persistantActiveEvent.Invoke);
            
            if (persistantCompleteEvent.GetPersistentEventCount() != 0)
                SequenceBuilder.Add(SectionType.Complete,"PersistantEvent", persistantCompleteEvent.Invoke);
        }

        public void Clear() => sequencer.Clear();
    }
}
