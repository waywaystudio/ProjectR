using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StopBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer sequencer;

        public CharacterActionMask BehaviourMask => CharacterActionMask.Stop;
        
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public void Stop() => sequencer.Active();
        public void Cancel() => sequencer.Cancel();

        private void OnEnable()
        {
            sequencer.ActiveAction.Add("CommonStopAction", () =>
            {
                Cb.Pathfinding.Stop();
                Cb.Animating.Idle();
                Cb.CurrentBehaviour = this;
            });
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
