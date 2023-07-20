using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class KnockBackBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private bool isImmune;
        
        public ActionMask BehaviourMask => ActionMask.KnockBack;
        public Sequencer<Vector3> Sequence { get; } = new();
        public SequenceBuilder<Vector3> Builder { get; private set; }
        public SequenceInvoker<Vector3> Invoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void KnockBack(Vector3 source, float distance, float duration)
        {
            if (!Invoker.IsAbleToActive) return;

            Cb.Pathfinding.KnockBack(source, distance, duration, Invoker.Complete);
            Invoker.Active(source);
        }
        
        public void Cancel() => Invoker.Cancel();
        public void Immune(bool value) => isImmune = value;
        

        private void OnEnable()
        {
            Builder = new SequenceBuilder<Vector3>(Sequence);
            Invoker = new SequenceInvoker<Vector3>(Sequence);

            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                .AddCondition("IsNotImmune", () => !isImmune)
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
