using Cysharp.Threading.Tasks;
using Sequences;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private Sequencer<float> sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.Stun;
        public FloatEvent RemainStunTime { get; } = new();
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.StunIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.StunIgnoreMask;
        

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
        
        public void StunSetDurationActiveParam(float duration)
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
            sequencer.ActiveParamSection.AddAwait("AwaitCoolTime", AwaitCoolTime);
        }
        
        private void OnDestroy()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer.AssignPersistantEvents();
        }
#endif
    }
}
