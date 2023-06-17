using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class RunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<Vector3> sequencer;

        public ActionMask BehaviourMask => ActionMask.Run;
        public SequenceBuilder<Vector3> SequenceBuilder { get; } = new();
        public SequenceInvoker<Vector3> SequenceInvoker { get; } = new();

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
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .AddCondition("CanMove", () => Cb.Pathfinding.CanMove)
                           .AddActiveParam("RunPathfinding", destination => Cb.Pathfinding.Move(destination, SequenceInvoker.Complete))
                           .AddActive("CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToCancel(this))
                           .AddActive("SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .AddActive("PlayAnimation", Cb.Animating.Run)
                           .AddEnd("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
