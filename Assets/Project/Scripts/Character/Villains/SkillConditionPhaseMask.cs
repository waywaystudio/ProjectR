using Common;
using UnityEngine;

namespace Character.Villains
{
    public class SkillConditionPhaseMask : MonoBehaviour
    {
        [SerializeField] private VillainPhaseMask enableMask;
        
        private VillainBehaviour villain;
        private ISequencer skill;

        private void OnEnable()
        {
            villain = GetComponentInParent<VillainBehaviour>();
            
            TryGetComponent(out skill);
            
            skill.Sequencer.Condition.Add("ConditionSelfHpStatus", () => (enableMask | villain.CurrentPhase.PhaseMask) == enableMask);
        }
    }
}
