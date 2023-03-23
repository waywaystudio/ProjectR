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

            trapComponent.OnActivated.Register("TimeTriggerOn", StartProgress);
            trapComponent.OnCanceled.Register("TimeTriggerOff", StopProgress);
            trapComponent.OnEnded.Register("TimeTriggerOff", StopProgress);
            trapComponent.ProlongTime = prolongTimer;
        }

        private void Update()
        {
            if (Progress.Value < prolongTimer)
            {
                Progress.Value += Time.deltaTime;
            }
            else
            {
                trapComponent.Complete();
                StopProgress();
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
