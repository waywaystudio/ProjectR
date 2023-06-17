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

            trapComponent.TrapSequencer.ActiveAction.Add("TimeTriggerOn", StartProgress);
            trapComponent.TrapSequencer.EndAction.Add("TimeTriggerOff", StopProgress);
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
                trapComponent.TrapSequencer.Complete();
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
