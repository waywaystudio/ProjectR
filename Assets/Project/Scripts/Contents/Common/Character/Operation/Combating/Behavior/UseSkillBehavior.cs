using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskCategory("Character/Combat")]
    public class UseSkillBehavior : Action
    {
        public SharedInt TargetSkillID;
        
        private Combat combat;
        private CharacterBehaviour cb;

        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
            cb = combat.Cb;
        }

        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            // Remove When All Condition Check Function Implemented
            if (skill.IsSkillReady)
            {
                combat.UseSkill(skill);
                return TaskStatus.Success;
            }
            
            Debug.Log($"{skill.SkillName} is Not Ready");
            return TaskStatus.Failure;
        }
    }
}
