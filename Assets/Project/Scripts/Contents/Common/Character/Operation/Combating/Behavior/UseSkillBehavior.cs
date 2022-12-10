using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskCategory("Character/Combat")]
    public class UseSkillBehavior : Action
    {
        private Combat combat;

        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
        }

        public override TaskStatus OnUpdate()
        {
            var anySkillReady = !combat.IsGlobalCooling
                                && combat.IsCoolOnAnySkill;

            if (!anySkillReady)
            {
                return TaskStatus.Failure;
            }

            if (combat.CurrentSkill != null && !combat.CurrentSkill.IsSkillFinished)
            {
                Debug.Log($"Return by {combat.CurrentSkill.SkillName} is Not Finished");
                return TaskStatus.Failure;
            }

            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }
            
            Debug.Log($"Most Priority Skill is : {skill.SkillName}");

            if (skill.IsSkillReady)
            {
                combat.UseSkill(skill);
                return TaskStatus.Success;
            }

            // 거리 문제일 수도 있고, 마나 문제일 수도 있고...
            // IsSkillReady가 False면 하나하나 조건 별로 잡아줘야 하고
            // 잡는 과정에서 아 그냥 넘기는게 좋겠다 하면 False로


            return combat.IsGlobalCooling 
                    ? TaskStatus.Failure
                    : TaskStatus.Success;
        }
    }
}
