using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combating.Entity;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskIcon("{SkinColor}IdleIcon.png"), TaskCategory("Character/Combat")]
    public class CoolTimeWaitBehavior : Action
    {
        private Combat combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
        }
        
        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            if (!skill.TryGetEntity<CoolTimeEntity>(EntityType.CoolTime, out var coolTimeEntity))
            {
                return TaskStatus.Success;
            }

            return coolTimeEntity.IsReady
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
