using UnityEngine;

namespace Common.Traps
{
    public class OverTimeExecutor : MonoBehaviour
    {
        [SerializeField] private float interval = 1f;

        private TrapComponent trapComponent;
        
        public FloatEvent Progress { get; } = new();


        private void OverTimeOn()
        {
            Progress.Value = interval;
            enabled        = true;
        }

        private void OverTimeOff()
        {
            Progress.Value = 0f;
            enabled        = false;
        }

        private void Awake()
        {
            TryGetComponent(out trapComponent);
            
            Progress.Value = interval;
            enabled        = false;

            // Require Builder - Sequencer and create new Builder
            trapComponent.Builder
                         .Add(Section.Active,"OverTimeOn", OverTimeOn)
                         .Add(Section.End,"OverTimeOff", OverTimeOff);
        }

        private void Update()
        {
            if (Progress.Value < interval)
            {
                Progress.Value += Time.deltaTime;
            }
            else
            {
                // Require Invoker
                trapComponent.Invoker.Execute();
                Progress.Value = 0f;
            }
        }
    }
}
