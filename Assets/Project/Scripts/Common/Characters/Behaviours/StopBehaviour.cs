using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StopBehaviour : MonoBehaviour, IActionBehaviour
    {
        public ActionMask BehaviourMask => ActionMask.Stop;
        public Sequencer Sequence { get; } = new();
        public SequenceBuilder Builder { get; private set; }
        public SequenceInvoker Invoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        public void Stop()
        {
            if (!Invoker.IsAbleToActive) return;
            
            Invoker.Active();
        }
        
        public void Cancel() => Invoker.Cancel();

        private void OnEnable()
        {
            Invoker = new SequenceInvoker(Sequence);
            Builder = new SequenceBuilder(Sequence);
            Builder
                .Add(Section.Active,"Cb.Pathfinding.Stop", Cb.Pathfinding.Stop)
                .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active,"PlayAnimation", Cb.Animating.Idle);
        }

        private void OnDisable()
        {
            Sequence.Clear();
        }
    }
}
