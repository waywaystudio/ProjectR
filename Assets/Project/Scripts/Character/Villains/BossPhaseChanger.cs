using BehaviorDesigner.Runtime.Tasks;

namespace Character.Villains
{
    [TaskCategory("Character/Villain")]
    public class BossPhaseChanger : Action
    {
        private VillainBehaviour boss;
        
        public override void OnAwake()
        {
            boss = gameObject.GetComponentInParent<VillainBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            boss.CurrentPhase.TryToNextPhase();

            return TaskStatus.Failure;
        }
    }
}
