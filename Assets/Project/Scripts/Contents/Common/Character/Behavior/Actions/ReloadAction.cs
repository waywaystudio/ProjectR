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
        public SharedBool IsReloaded = true;
        public float WaitDuration;
        private float pauseTime;

        public override void OnAwake()
        {
            WaitDuration = WaitTime.Value;
            IsReloaded = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (WaitDuration <= 0.0f)
            {
                IsReloaded = true;
            }
            else
                WaitDuration -= Time.deltaTime;

            return TaskStatus.Success;
        }

        public override void OnPause(bool paused)
        {
            if (paused)
            {
                pauseTime = Time.time;
            }
            else
            {
                WaitDuration += Time.time - pauseTime;
            }
        }

        public override void OnReset()
        {
            WaitDuration = WaitTime.Value;
        }
    }
}
