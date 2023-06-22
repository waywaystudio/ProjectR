using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class KnockBackBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<Vector3> sequencer;        
        
        public ActionMask BehaviourMask => ActionMask.KnockBack;
        public SequenceBuilder<Vector3> Builder { get; private set; }
        public SequenceInvoker<Vector3> SequenceInvoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void KnockBack(Vector3 source, float distance, float duration)
        {
            if (!SequenceInvoker.IsAbleToActive) return;

            Cb.Pathfinding.KnockBack(source, distance, duration, SequenceInvoker.Complete);
            SequenceInvoker.Active(source);
        }
        
        public void Cancel() => SequenceInvoker.Cancel();


        private void OnEnable()
        {
            Builder         = new SequenceBuilder<Vector3>(sequencer);
            SequenceInvoker = new SequenceInvoker<Vector3>(sequencer);

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
