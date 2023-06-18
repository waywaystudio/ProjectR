using UnityEngine;

namespace Common.Traps
{
    public class OverTimeExecutor : MonoBehaviour, IEditable
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

            trapComponent.SequenceBuilder
                         .Add(SectionType.Active,"OverTimeOn", OverTimeOn)
                         .Add(SectionType.End,"OverTimeOff", OverTimeOff);
        }

        private void Update()
        {
            if (Progress.Value < interval)
            {
                Progress.Value += Time.deltaTime;
            }
            else
            {
                trapComponent.Execution();
                Progress.Value = 0f;
            }
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            // Get Interval from Database
        }
#endif
    }
}
