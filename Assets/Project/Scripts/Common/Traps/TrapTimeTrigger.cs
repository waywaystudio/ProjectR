using UnityEngine;

namespace Common.Traps
{
    public class TrapTimeTrigger : MonoBehaviour
    {
        [SerializeField] protected float prolongTimer;

        private TrapComponent trapComponent;
        
        public FloatEvent Progress { get; } = new();

        
        private void StartProgress()
        {
            Progress.Value = 0f;
            enabled        = true;
        }

        private void StopProgress()
        {
            Progress.Value = 0f;
            enabled        = false;
        }
        
        private void Awake()
        {
            TryGetComponent(out trapComponent);

            // Require Builder
            trapComponent.ProlongTime = prolongTimer;
            trapComponent.SequenceBuilder
                         .Add(Section.Active,"TimeTriggerOn", StartProgress)
                         .Add(Section.End,"TimeTriggerOff", StopProgress);
        }

        private void Update()
        {
            if (Progress.Value < prolongTimer)
            {
                Progress.Value += Time.deltaTime;
            }
            else
            {
                trapComponent.SequenceInvoker.Complete();
            }
        }
    }
}
