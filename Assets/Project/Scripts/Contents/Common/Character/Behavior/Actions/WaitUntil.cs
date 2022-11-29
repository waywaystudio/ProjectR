using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace Common.Character.Behavior.Actions
{
    [System.Serializable]
    [TaskIcon("{SkinColor}WaitIcon.png"), TaskCategory("Character")]
    public class WaitUntil : Action
    {
        [Tooltip("The amount of time to wait")]
        public SharedFloat WaitTime = 1; 
        
        private float waitDuration;
        private float startTime;
        private float pauseTime;

        public override void OnStart()
        {
            startTime = Time.time;
            waitDuration = WaitTime.Value;
        }

        public override TaskStatus OnUpdate() 
            => startTime + waitDuration < Time.time 
            ? TaskStatus.Success 
            : TaskStatus.Running;

        public override void OnPause(bool paused)
        {
            if (paused)
            {
                pauseTime = Time.time;
            }
            else
            {
                startTime += Time.time - pauseTime;
            }
        }
    }
}
