using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
    [TaskIcon("{SkinColor}WaitIcon.png")]
    public class Wait : Action
    {
        [Tooltip("The amount of time to wait")]
        public SharedFloat WaitTime = 1;
        [Tooltip("Should the wait be randomized?")]
        public SharedBool RandomWait = false;
        [Tooltip("The minimum wait time if random wait is enabled")]
        public SharedFloat RandomWaitMin = 1;
        [Tooltip("The maximum wait time if random wait is enabled")]
        public SharedFloat RandomWaitMax = 1;

        private float waitDuration;
        private float startTime;
        private float pauseTime;

        public override void OnStart()
        {
            // Remember the start time.
            startTime = Time.time;
            if (RandomWait.Value) {
                waitDuration = Random.Range(RandomWaitMin.Value, RandomWaitMax.Value);
            } else {
                waitDuration = WaitTime.Value;
            }
        }

        public override TaskStatus OnUpdate()
        {
            // The task is done waiting if the time waitDuration has elapsed since the task was started.
            if (startTime + waitDuration < Time.time) {
                return TaskStatus.Success;
            }
            // Otherwise we are still waiting.
            return TaskStatus.Running;
        }

        public override void OnPause(bool paused)
        {
            if (paused) {
                // Remember the time that the behavior was paused.
                pauseTime = Time.time;
            } else {
                // Add the difference between Time.time and pauseTime to figure out a new start time.
                startTime += (Time.time - pauseTime);
            }
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            WaitTime = 1;
            RandomWait = false;
            RandomWaitMin = 1;
            RandomWaitMax = 1;
        }
    }
}