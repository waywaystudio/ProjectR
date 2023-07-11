using BehaviorDesigner.Runtime.Tasks;
using Common.Characters.Behaviours;
using UnityEngine;

namespace Common.Behavior.Actions
{
    [TaskIcon("{SkinColor}SelectorIcon.png"), TaskCategory("Character/Combat")]
    public class IsGcdReady : Action
    {
        private SkillTable sb;
        
        public override void OnAwake()
        {
            TryGetComponent(out sb);
            
            if (sb.IsNullOrDestroyed())
            {
                Debug.LogError("Not Exist SkillBehaviour for Global CoolTimer");
            }
        }
        
        public override TaskStatus OnUpdate() => 
            //sb.IsGlobalCoolTimeReady
            // ? 
        TaskStatus.Success
            // : TaskStatus.Failure
            ;
    }
}
