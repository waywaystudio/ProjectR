using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Villains
{
    public class SkillConditionPhaseMask : MonoBehaviour
    {
        [SerializeField] private VillainPhaseMask enableMask;
        
        private VillainBehaviour villain;
        private SkillComponent skill;

        private void OnEnable()
        {
            villain = GetComponentInParent<VillainBehaviour>();
            
            TryGetComponent(out skill);

            skill.Initialize();
            skill.SequenceBuilder
                 .AddCondition("ConditionSelfHpStatus", () => (enableMask | villain.CurrentPhase.PhaseMask) == enableMask);
        }
    }
}
