using Common;
using UnityEngine;

namespace Character.Villains
{
    public class SkillConditionPhaseMask : MonoBehaviour
    {
        [SerializeField] private VillainPhaseMask enableMask;
        
        private VillainBehaviour boss;
        private IOldConditionalSequence sequence;

        private void OnEnable()
        {
            boss = GetComponentInParent<VillainBehaviour>();
            
            TryGetComponent(out sequence);
            
            sequence.Conditions.Register("ConditionSelfHpStatus", () => (enableMask | boss.CurrentPhase.PhaseFlag) == enableMask);
        }
    }
}
