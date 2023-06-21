using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StopBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer sequencer = new();

        public ActionMask BehaviourMask => ActionMask.Stop;
        public SequenceBuilder SequenceBuilder { get; } = new();
        public SequenceInvoker SequenceInvoker { get; } = new();

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void Stop()
        {
            if (!SequenceInvoker.IsAbleToActive) return;
            
            SequenceInvoker.Active();
        }
        
        public void Cancel() => SequenceInvoker.Cancel();

        private void OnEnable()
        {
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .Add(SectionType.Active,"Cb.Pathfinding.Stop", Cb.Pathfinding.Stop)
                           .Add(SectionType.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .Add(SectionType.Active,"PlayAnimation", Cb.Animating.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
