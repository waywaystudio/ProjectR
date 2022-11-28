using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Common.Character.Behavior
{
    [System.Serializable]
    public class WaitUntil : Action
    {
        private float waitDuration;
        private float startTime;
        private float pauseTime;

        public override void OnStart()
        {
            startTime = Time.time;
        }
    }
}
