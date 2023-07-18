using BehaviorDesigner.Runtime.Tasks;
using Common.Characters;
using UnityEngine;

namespace Common.Behavior.Characters
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Characters")]
    public class OnRigidAction : Action
    {
        private CharacterBehaviour cb;
        
        public override void OnAwake()
        {
            cb = gameObject.GetComponentInParent<CharacterBehaviour>();
        }
        
        public override TaskStatus OnUpdate()
        {
            return cb.IsRigid
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
