using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Common.Character.Player;
using Core;
using UnityEngine;

namespace Common.Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}UI_Icon_Attack.png")]
    [TaskCategory("Character")]
    public class AttackAction : Action
    {
        private PlayerBehaviour pb;

        public override void OnAwake()
        {
            pb = GetComponent<PlayerBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            var distance = Vector3.Distance(pb.transform.position, pb.FocusTarget.transform.position);

            if (pb.Range >= distance && !pb.FocusTarget.IsNullOrEmpty())
            {
                pb.Attack(pb.FocusTarget);
                return TaskStatus.Success;
            }
           
            return TaskStatus.Failure;
          
        }
    }
}
