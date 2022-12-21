using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskCategory("Character/Combat")]
    public class UseSkillBehavior : Action
    {
        public SharedInt TargetSkillID;
        
        private Combating combat;
        

        public override void OnAwake()
        {
            combat = GetComponent<Combating>();
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

            return TaskStatus.Failure;
        }
    }
}
