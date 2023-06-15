using System;
using Common;
using UnityEngine;
using UnityEngine.Events;

namespace Character.Villains
{
    [Serializable]
    public class PhaseSequencer : Sequencer
    {
        [SerializeField] private UnityEvent persistantActiveEvent;
        [SerializeField] private UnityEvent persistantCompleteEvent;
        [SerializeField] private VillainPhaseMask phaseMask;

        public VillainPhaseMask PhaseMask => phaseMask;

        public void Initialize()
        {
            if (persistantActiveEvent.GetPersistentEventCount() != 0)
                ActiveAction.Add("PersistantEvent", persistantActiveEvent.Invoke);
            
            if (persistantCompleteEvent.GetPersistentEventCount() != 0)
                CompleteAction.Add("PersistantEvent", persistantCompleteEvent.Invoke);
        }
    }
}
