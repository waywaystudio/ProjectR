using Common;
using Sequences;
using UnityEngine;

namespace Character.Villains
{
    public class PhaseSequencer : Sequencer
    {
        [SerializeField] private VillainPhaseMask phaseMask;

        public VillainPhaseMask PhaseMask => phaseMask;
    }
}
