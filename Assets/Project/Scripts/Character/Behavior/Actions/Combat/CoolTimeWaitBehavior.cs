using BehaviorDesigner.Runtime.Tasks;
using Character.Combat;

namespace Character.Behavior.Actions.Combat
{
    [TaskIcon("{SkinColor}IdleIcon.png"), TaskCategory("Character/Combat")]
    public class CoolTimeWaitBehavior : Action
    {
        private CombatBehaviour combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<CombatBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            if (skill.CoolTimeModule is null)
            {
                return TaskStatus.Success;
            }

            return skill.CoolTimeModule.IsReady
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
