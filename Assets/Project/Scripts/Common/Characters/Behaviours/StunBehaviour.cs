using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour
    {
        public ActionMask BehaviourMask => ActionMask.Stun;
        public Sequencer<float> Sequencer { get; } = new();
        public SequenceBuilder<float> Builder { get; private set; }
        public SequenceInvoker<float> Invoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private CastTimer Timer { get; } = new();
        

        public void Stun(float duration)
        {
            if (!Invoker.IsAbleToActive) return;
            
            Invoker.Active(duration);
        }

        public void Cancel()
        {
            Invoker.Cancel();
            Timer.Stop();
        }


        private void OnEnable()
        {
            Invoker = new SequenceInvoker<float>(Sequencer);
            Builder = new SequenceBuilder<float>(Sequencer);
            Builder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                   .AddActiveParam("PlayStunTimer", duration => Timer.Play(duration, Invoker.Complete))
                   .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                   .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                   .Add(Section.Active,"PlayAnimation", Cb.Animating.Stun)
                   .Add(Section.End,"Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            Sequencer.Clear();
            Timer.Dispose();
        }
    }
}
