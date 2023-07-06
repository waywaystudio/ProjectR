using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StopBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer sequencer = new();

        public ActionMask BehaviourMask => ActionMask.Stop;
        public SequenceBuilder SequenceBuilder { get; private set; }
        public SequenceInvoker SequenceInvoker { get; private set; }

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
            SequenceInvoker = new SequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder(sequencer);
            SequenceBuilder.Add(Section.Active,"Cb.Pathfinding.Stop", Cb.Pathfinding.Stop)
                           .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .Add(Section.Active,"PlayAnimation", Cb.Animating.Idle);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
