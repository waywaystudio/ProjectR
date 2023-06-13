using Common;
using UnityEngine;

namespace Character.Villains
{
    public class SkillConditionPhaseMask : MonoBehaviour
    {
        [SerializeField] private VillainPhaseMask enableMask;
        
        private VillainBehaviour villain;
        private IOldConditionalSequence sequence;

        private void OnEnable()
        {
            villain = GetComponentInParent<VillainBehaviour>();
            
            TryGetComponent(out sequence);
            
            sequence.Conditions.Add("ConditionSelfHpStatus", () => (enableMask | villain.CurrentPhase.PhaseMask) == enableMask);
        }
    }
}
