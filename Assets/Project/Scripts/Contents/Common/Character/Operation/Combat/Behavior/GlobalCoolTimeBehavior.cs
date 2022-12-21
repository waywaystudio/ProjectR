using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class GlobalCoolTimeBehavior : Action
    {
        private Combating combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combating>();
        }

        public override TaskStatus OnUpdate() => combat.IsGlobalCooling
            ? TaskStatus.Failure 
            : TaskStatus.Success;
    }
}
