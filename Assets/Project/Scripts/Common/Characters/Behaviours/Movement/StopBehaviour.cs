using UnityEngine;

namespace Common.Characters.Behaviours.Movement
{
    public class StopBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private StopSequencer sequencer;
        
        private CharacterBehaviour cb;

        public CharacterActionMask BehaviourMask => CharacterActionMask.Stop;
        public CharacterActionMask IgnorableMask => CharacterActionMask.None;
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void Stop() => sequencer.Active();
        public void Cancel() => sequencer.Cancel();
                                // Cancellation.Invoke();
        
        public void StopActive()
        {
            Cb.Pathfinding.Stop();
            Cb.Animating.Idle();
            Cb.CurrentBehaviour = this;
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer = GetComponentInChildren<StopSequencer>();
        }
#endif
    }
}
