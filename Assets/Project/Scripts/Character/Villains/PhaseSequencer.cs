using System;
using Common;
using Sequences;
using UnityEngine;

namespace Character.Villains
{
    [Serializable]
    public class PhaseSequencer : Sequencer
    {
        [SerializeField] private VillainPhaseMask phaseMask;

        public VillainPhaseMask PhaseMask => phaseMask;
    }
}
