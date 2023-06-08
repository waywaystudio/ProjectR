using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Character.Villains
{
    public class VillainPhaseTable : MonoBehaviour, IEditable
    {
        [SerializeField] private List<BossPhase> phaseList;

        public BossPhase GetStartPhase()
        {
            BossPhase result = null;
            
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
        
        public BossPhase GetPhase(int index)
        {
            foreach (var phase in phaseList)
            {
                if (phase.Index == index) return phase;
            }

            return null;
        }

        public List<BossPhase> GetPhaseList(BossPhaseMask mask)
        {
            var result = new List<BossPhase>();
            
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
