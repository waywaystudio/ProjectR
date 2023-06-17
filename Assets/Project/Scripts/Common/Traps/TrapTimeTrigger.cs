using UnityEngine;

namespace Common.Traps
{
    public class TrapTimeTrigger : MonoBehaviour, IEditable
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

            trapComponent.ProlongTime = prolongTimer;
            trapComponent.SequenceBuilder
                         .AddActive("TimeTriggerOn", StartProgress)
                         .AddEnd("TimeTriggerOff", StopProgress);
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


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            // Get prolongTimer from Database
        }
#endif
    }
}
