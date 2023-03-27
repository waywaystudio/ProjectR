using BehaviorDesigner.Runtime.Tasks;

namespace Character.Bosses
{
    [TaskCategory("Character/Boss/PhaseChanger")]
    public class BossPhaseChanger : Action
    {
        private Boss boss;
        
        public override void OnAwake()
        {
            boss = gameObject.GetComponentInParent<Boss>();
        }
        
        public override TaskStatus OnUpdate()
        {
            boss.CurrentPhase.TryToNextPhase();

            return TaskStatus.Failure;
        }
    }
}
