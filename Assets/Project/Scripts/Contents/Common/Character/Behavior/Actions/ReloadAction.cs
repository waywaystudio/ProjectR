using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
using UnityEngine;

namespace Common.Character.Behavior.Actions
{
    [TaskCategory("Character")]
    public class ReloadAction : Action
    {
        [Tooltip("The amount of time to wait")]
        public SharedFloat WaitTime = 1;
        
        private float waitDuration;
        private float pauseTime;

        public override void OnAwake()
        {
            waitDuration = WaitTime.Value;
        }

        public override TaskStatus OnUpdate()
        {
            if (waitDuration <= 0.0f)
            {
                return TaskStatus.Success;
            }
            
            waitDuration -= Time.deltaTime;
                
            return TaskStatus.Failure;
        }

        public override void OnPause(bool paused)
        {
            if (paused)
            {
                pauseTime = Time.time;
            }
            else
            {
                waitDuration += Time.time - pauseTime;
            }
        }

        public override void OnReset()
        {
            waitDuration = WaitTime.Value;
        }
    }
}
