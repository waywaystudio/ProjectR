using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class DrawBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<Vector3> sequencer;        
        
        public ActionMask BehaviourMask => ActionMask.Draw;
        public SequenceBuilder<Vector3> Builder { get; private set; }
        public SequenceInvoker<Vector3> Invoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void Draw(Vector3 source, float duration)
        {
            if (!Invoker.IsAbleToActive) return;

            Cb.Pathfinding.Draw(source, duration, Invoker.Complete);
            Invoker.Active(source);
        }
        
        public void Cancel() => Invoker.Cancel();


        private void OnEnable()
        {
            Builder = new SequenceBuilder<Vector3>(sequencer);
            Invoker = new SequenceInvoker<Vector3>(sequencer);

            Builder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                   .AddActiveParam("Rotate", Cb.Rotate)
                   .Add(SectionType.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                   .Add(SectionType.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                   .Add(SectionType.Active,"PlayAnimation", Cb.Animating.Hit)
                   .Add(SectionType.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
