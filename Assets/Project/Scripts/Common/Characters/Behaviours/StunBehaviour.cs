using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<float> sequencer = new();

        public ActionMask BehaviourMask => ActionMask.Stun;
        public SequenceBuilder<float> SequenceBuilder { get; } = new();
        public SequenceInvoker<float> SequenceInvoker { get; } = new();

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private AwaitTimer Timer { get; } = new();
        

        public void Stun(float duration)
        {
            if (!SequenceInvoker.IsAbleToActive) return;
            
            SequenceInvoker.Active(duration);
        }

        public void Cancel()
        {
            SequenceInvoker.Cancel();
            Timer.Stop();
        }


        private void OnEnable()
        {
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .AddActiveParam("PlayStunTimer", duration => Timer.Play(duration, sequencer.Complete))
                           .AddActive("CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToCancel(this))
                           .AddActive("SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .AddActive("PlayAnimation", Cb.Animating.Stun)
                           .AddEnd("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
            Timer.Dispose();
        }
    }
}
