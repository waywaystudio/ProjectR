using BehaviorDesigner.Runtime.Tasks;
using Monsters;
using UnityEngine;

namespace Character.Bosses
{
    [TaskCategory("Character/Boss/PhaseChanger")]
    public class BossPhaseChanger : Action
    {
        private Boss boss;
        private BossPhaseTable bp;
        
        public override void OnAwake()
        {
            TryGetComponent(out boss);
        }
        
        public override TaskStatus OnUpdate()
        {
            if (bp.IsAbleToNextPhase())
            {
                Debug.Log("Phase Changed");
                bp.ActiveNextPhase();
            }

            return TaskStatus.Failure;
        }
    }
}
