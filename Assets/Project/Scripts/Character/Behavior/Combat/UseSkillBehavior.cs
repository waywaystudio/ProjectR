using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Combat
{
    [TaskCategory("Character/Combat")]
    public class UseSkillBehavior : Action
    {
        private CharacterBehaviour cb;
        private OldSkillBehaviour combat;

        public override void OnAwake()
        {
            cb     = gameObject.GetComponentInParent<CharacterBehaviour>();
            combat = GetComponent<OldSkillBehaviour>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }
            
            cb.UseSkill(skill);
                
            return TaskStatus.Success;

            // Remove When All Condition Check Function Implemented
            // if (skill.IsSkillReady)
            // {
            //     cb.UseSkill(skill);
            //     
            //     return TaskStatus.Success;
            // }
            //
            // return TaskStatus.Failure;
        }
    }
}
