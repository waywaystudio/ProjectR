using BehaviorDesigner.Runtime.Tasks;

namespace Character.Villains
{
    [TaskCategory("Character/Villain")]
    public class BossPhaseChanger : Action
    {
        private VillainBehaviour villain;
        
        public override void OnAwake()
        {
            villain = gameObject.GetComponentInParent<VillainBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            villain.CheckPhaseBehaviour();
                // CurrentPhase.TryToNextPhase();

            return TaskStatus.Failure;
        }
    }
}
