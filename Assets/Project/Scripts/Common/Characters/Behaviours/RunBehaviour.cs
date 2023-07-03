using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class RunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<Vector3> sequencer;

        public ActionMask BehaviourMask => ActionMask.Run;
        public SequenceBuilder<Vector3> Builder { get; private set; }
        public SequenceInvoker<Vector3> SequenceInvoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void Run(Vector3 destination)
        {
            if (!SequenceInvoker.IsAbleToActive) return;
            
            SequenceInvoker.Active(destination);
        }

        public void Cancel() => SequenceInvoker.Cancel();
        

        private void OnEnable()
        {
            Builder         = new SequenceBuilder<Vector3>(sequencer);
            SequenceInvoker = new SequenceInvoker<Vector3>(sequencer);

            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                .AddCondition("CanMove", () => Cb.Pathfinding.CanMove)
                .AddActiveParam("RunPathfinding", destination => Cb.Pathfinding.Move(destination, SequenceInvoker.Complete))
                .Add(SectionType.Active, "PlayAnimation", Cb.Animating.Run)
                .Add(SectionType.Active, "CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(SectionType.Active, "SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(SectionType.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
