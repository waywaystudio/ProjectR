using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using Core;

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

            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

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


// 일단 두 가지만 생각해보자.
// 1. CommonAttack
// 2. AimShot
// 에임샷 쏘려는데 거리가 안되면 이동시키고
// 도착하기 전 까지 대기하고
// 도착하면 에임샷 쏘고, BD 재시작.
// 에임샷을 쿨 안되니 넘기고, 
// CommonAttack 가능하니 공격
// BD 재시작
// 에임샷을 쿨 안되니 넘기고, 
// CommonAttack 가능하니 공격
// BD 재시작
// 에임샷을 쿨 안되니 넘기고, 
// CommonAttack 가능하니 공격
// BD 재시작
// 에임샷 쿨 왔으니 공격
// BD 재시작