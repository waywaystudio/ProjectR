using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<float> sequencer;

        public CharacterActionMask BehaviourMask => CharacterActionMask.Stun;

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private AwaitTimer Timer { get; } = new();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.StunIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.StunIgnoreMask;
        

        public void Stun(float duration)
        {
            if (!sequencer.IsAbleToActive) return;
            
            sequencer.Active(duration);
        }

        public void Cancel() => sequencer.Cancel();


        private void OnEnable()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveParamAction.Add("CommonStunAction", duration => Timer.Play(duration, sequencer.Complete));
            sequencer.ActiveAction.Add("CommonStunAction", () =>
            {
                if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
                    cb.CurrentBehaviour.Cancel();

                cb.CurrentBehaviour = this;
                Cb.Animating.Stun();
            });
            
            sequencer.EndAction.Add("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
