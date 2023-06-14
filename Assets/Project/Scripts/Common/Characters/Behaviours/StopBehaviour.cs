using Sequences;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StopBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private Sequencer sequencer;

        private CharacterBehaviour cb;

        public CharacterActionMask BehaviourMask => CharacterActionMask.Stop;
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void Stop() => sequencer.Active();
        public void Cancel() => sequencer.Cancel();

        public void StopPathfindingActive() => Cb.Pathfinding.Stop();
        public void StopAnimationActive() => Cb.Animating.Idle();
        public void StopRegisterActive() => Cb.CurrentBehaviour = this;
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer.AssignPersistantEvents();
        }
#endif
    }
}
