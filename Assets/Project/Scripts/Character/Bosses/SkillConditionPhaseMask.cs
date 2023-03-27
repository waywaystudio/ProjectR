using Common;
using UnityEngine;

namespace Character.Bosses
{
    public class SkillConditionPhaseMask : MonoBehaviour
    {
        [SerializeField] private BossPhaseMask enableMask;
        
        private Boss boss;
        private IConditionalSequence sequence;

        private void OnEnable()
        {
            boss = GetComponentInParent<Boss>();
            
            TryGetComponent(out sequence);
            
            sequence.Conditions.Register("ConditionSelfHpStatus", () => (enableMask | boss.CurrentPhase.PhaseFlag) == enableMask);
        }
    }
}
