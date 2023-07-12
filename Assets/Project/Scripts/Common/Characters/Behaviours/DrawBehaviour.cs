using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class DrawBehaviour : MonoBehaviour, IActionBehaviour
    {
        public ActionMask BehaviourMask => ActionMask.Draw;
        public Sequencer<Vector3> Sequence { get; } = new();
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
            Builder = new SequenceBuilder<Vector3>(Sequence);
            Invoker = new SequenceInvoker<Vector3>(Sequence);
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                .AddActiveParam("Rotate", Cb.Rotate)
                .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active,"PlayAnimation", Cb.Animating.Hit)
                .Add(Section.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            Sequence.Clear();
        }
    }
}
