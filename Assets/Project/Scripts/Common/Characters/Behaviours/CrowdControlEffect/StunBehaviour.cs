using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private StunSequencer sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.Stun;
        public CharacterActionMask IgnorableMask => CharacterActionMask.StunIgnoreMask;
        public FloatEvent RemainStunTime { get; } = new();
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask;
        

        public void Stun(float duration)
        {
            if (!sequencer.IsAbleToActive) return;
            
            sequencer.Activate(duration);
        }

        public void Cancel() 
            => sequencer.Cancel();

        public void StunRegisterActive()
        {
            if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
            {
                cb.CurrentBehaviour.Cancel();
            }

            cb.CurrentBehaviour = this;
        }
        
        public void StunAnimationActive()
        {
            Cb.Animating.Stun();
        }
        
        public void StunSetDurationActive(float duration)
        {
            RemainStunTime.Value = duration;
        }

        public void StunCancel()
        {
            Cb.Stop();
        }

        public void StunComplete()
        {
            Cb.Stop();
        }

        public void StunEnd()
        {
            RemainStunTime.Value = 0f;
        }


        private async UniTask AwaitCoolTime()
        {
            while (RemainStunTime.Value > 0)
            {
                RemainStunTime.Value -= Time.deltaTime;

                await UniTask.Yield();
            }
        }
        
        private void Awake()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveSection.AddAwait("AwaitCoolTime", AwaitCoolTime);
        }
        
        private void OnDestroy()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer = GetComponentInChildren<StunSequencer>();
        }
#endif
    }
}
