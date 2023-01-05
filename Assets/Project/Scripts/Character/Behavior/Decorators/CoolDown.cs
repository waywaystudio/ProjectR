using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Character.Behavior.Decorators
{
    [TaskIcon("{SkinColor}CooldownIcon.png"), TaskCategory("Character")]
    public class CoolDown : Decorator
    {
        public SharedFloat Duration = 2;

        private TaskStatus executionStatus = TaskStatus.Inactive;
        private float cooldownTime = -1;

        public override bool CanExecute()
        {
            if (cooldownTime.Equals(-1)) return true;

            return cooldownTime + Duration.Value > Time.time;
        }

        public override int CurrentChildIndex()
            => cooldownTime.Equals(-1)
            ? 0
            : -1;

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            executionStatus = childStatus;

            if (executionStatus == TaskStatus.Failure || executionStatus == TaskStatus.Success)
                cooldownTime = Time.time;
        }

        public override TaskStatus OverrideStatus()
            => !CanExecute()
            ? TaskStatus.Running
            : executionStatus;

        public override TaskStatus OverrideStatus(TaskStatus status)
            => status == TaskStatus.Running
            ? status
            : executionStatus;

        public override void OnEnd()
        {
            executionStatus = TaskStatus.Inactive;
            cooldownTime = -1;
        }
    }
}
