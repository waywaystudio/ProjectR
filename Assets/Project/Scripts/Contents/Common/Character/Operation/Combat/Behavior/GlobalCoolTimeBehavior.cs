using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class GlobalCoolTimeBehavior : Action
    {
        private Combat.Combating combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combat.Combating>();
        }

        public override TaskStatus OnUpdate() => combat.IsGlobalCooling
            ? TaskStatus.Failure 
            : TaskStatus.Success;
    }
}
