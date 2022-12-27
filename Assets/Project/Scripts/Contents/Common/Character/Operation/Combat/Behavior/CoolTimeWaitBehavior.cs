using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combat.Entity;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskIcon("{SkinColor}IdleIcon.png"), TaskCategory("Character/Combat")]
    public class CoolTimeWaitBehavior : Action
    {
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

            if (skill.CoolTimeEntity is null)
            {
                return TaskStatus.Success;
            }

            return skill.CoolTimeEntity.IsReady
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
