using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class RunBehaviour : MonoBehaviour, IActionBehaviour
    {
        public ActionMask BehaviourMask => ActionMask.Run;
        public Sequencer<Vector3> Sequence { get; } = new();
        public SequenceBuilder<Vector3> Builder { get; private set; }
        public SequenceInvoker<Vector3> Invoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void Run(Vector3 destination)
        {
            if (!Invoker.IsAbleToActive) return;
            
            Invoker.Active(destination);
        }

        public void Cancel() => Invoker.Cancel();
        

        private void OnEnable()
        {
            Builder = new SequenceBuilder<Vector3>(Sequence);
            Invoker = new SequenceInvoker<Vector3>(Sequence);

            /* 순서 중요 */ 
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                .AddCondition("CanMove", () => Cb.Pathfinding.CanMove)
                .AddActiveParam("RunPathfinding", destination => Cb.Pathfinding.Move(destination, Invoker.Complete))
                .Add(Section.Active, "CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active, "SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active, "PlayAnimation", () => Cb.Animating.Run(1.5f))
                .Add(Section.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            Sequence.Clear();
        }
    }
}
