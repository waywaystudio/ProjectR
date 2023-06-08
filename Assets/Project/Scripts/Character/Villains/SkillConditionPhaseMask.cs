using Common;
using UnityEngine;

namespace Character.Villains
{
    public class SkillConditionPhaseMask : MonoBehaviour
    {
        [SerializeField] private BossPhaseMask enableMask;
        
        private VillainBehaviour boss;
        private IConditionalSequence sequence;

        private void OnEnable()
        {
            boss = GetComponentInParent<VillainBehaviour>();
            
            TryGetComponent(out sequence);
            
            sequence.Conditions.Register("ConditionSelfHpStatus", () => (enableMask | boss.CurrentPhase.PhaseFlag) == enableMask);
        }
    }
}
