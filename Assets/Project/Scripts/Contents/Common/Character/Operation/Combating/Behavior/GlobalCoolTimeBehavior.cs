using BehaviorDesigner.Runtime.Tasks;

namespace Common.Character.Operation.Combating.Behavior
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class GlobalCoolTimeBehavior : Action
    {
        private Combat combat;
        
        public override void OnAwake()
        {
            combat = GetComponent<Combat>();
        }

        public override TaskStatus OnUpdate() => combat.IsGlobalCooling
            ? TaskStatus.Failure 
            : TaskStatus.Success;
    }
}
