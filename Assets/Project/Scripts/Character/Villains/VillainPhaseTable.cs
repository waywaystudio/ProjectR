using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Character.Villains
{
    public class VillainPhaseTable : MonoBehaviour, IEditable
    {
        [SerializeField] private List<VillainPhase> phaseList;

        public VillainPhase GetStartPhase()
        {
            VillainPhase result = null;
            
            foreach (var phase in phaseList)
            {
                if (result is null)
                {
                    result = phase;
                    continue;
                }

                if (phase.Index < result.Index)
                {
                    result = phase;
                }
            }

            return result;
        }
        
        public VillainPhase GetPhase(int index)
        {
            foreach (var phase in phaseList)
            {
                if (phase.Index == index) return phase;
            }

            return null;
        }

        public List<VillainPhase> GetPhaseList(VillainPhaseMask mask)
        {
            var result = new List<VillainPhase>();
            
            foreach (var phase in phaseList)
            {
                if ((mask | phase.PhaseFlag) == mask) result.Add(phase);
            }

            return result;
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, phaseList);
        }
#endif
    }
}
