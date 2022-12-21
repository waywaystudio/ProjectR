using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Operation.Combat.Entity;
using UnityEngine;

namespace Common.Character.Operation.Combat.Behavior
{
    [TaskCategory("Character/Combat")]
    public class PositioningBehavior : Action
    {
        private Combat.Combating combat;
        private CharacterBehaviour cb;
        

        public override void OnAwake()
        {
            combat = GetComponent<Combat.Combating>();
            cb = combat.Cb;
        }

        public override TaskStatus OnUpdate()
        {
            if (!combat.TryGetMostPrioritySkill(out var skill))
            {
                return TaskStatus.Failure;
            }

            if (!skill.TryGetEntity<TargetEntity>(EntityType.Target, out var targetEntity))
            {
                return TaskStatus.Success;
            }

            var isMovable = combat.CombatPosition.TryGetCombatPosition(
                targetEntity.CombatTaker, targetEntity.Range, out var destination);

            if (!isMovable && cb.IsReached.Invoke())
            {
                return TaskStatus.Success;
            }
            
            cb.Run(destination); // destination
            return TaskStatus.Failure;
        }
    }
}
