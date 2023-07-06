using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<float> sequencer = new();

        public ActionMask BehaviourMask => ActionMask.Stun;
        public SequenceBuilder<float> Builder { get; private set; }
        public SequenceInvoker<float> SequenceInvoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private CastTimer Timer { get; } = new();
        

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
            Builder         = new SequenceBuilder<float>(sequencer);
            SequenceInvoker = new SequenceInvoker<float>(sequencer);

            Builder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                   .AddActiveParam("PlayStunTimer", duration => Timer.Play(duration, SequenceInvoker.Complete))
                   .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                   .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                   .Add(Section.Active,"PlayAnimation", Cb.Animating.Stun)
                   .Add(Section.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
            Timer.Dispose();
        }
    }
}
