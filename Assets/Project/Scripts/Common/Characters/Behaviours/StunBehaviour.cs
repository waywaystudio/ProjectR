using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<float> sequencer = new();

        public ActionMask BehaviourMask => ActionMask.Stun;
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
            
            var builder = new SequenceBuilder<float>(sequencer);
            
            builder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                   .AddActiveParam("PlayStunTimer", duration => Timer.Play(duration, SequenceInvoker.Complete))
                   .Add(SectionType.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                   .Add(SectionType.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                   .Add(SectionType.Active,"PlayAnimation", Cb.Animating.Stun)
                   .Add(SectionType.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
            Timer.Dispose();
        }
    }
}
