using BehaviorDesigner.Runtime.Tasks;
using Core;
using UnityEngine;

namespace Common.Character.Behavior.Actions
{
    [TaskIcon("{SkinColor}UI_Icon_Attack.png")]
    [TaskCategory("Character")]
    public class AttackAction : Action
    {
        private OLD_CharacterBehaviour pb;

        public override void OnAwake()
        {
            pb = GetComponent<OLD_CharacterBehaviour>();
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
