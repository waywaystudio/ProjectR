using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class KnockBackBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private KnockBackSequencer sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.KnockBack;
        public CharacterActionMask IgnorableMask => CharacterActionMask.KnockBackIgnoreMask;
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask;
        

        public void KnockBack(Vector3 source, float distance, float duration)
        {
            if (!sequencer.IsAbleToActive) return;
            
            Cb.Pathfinding.SetKnockProperty(distance, duration);
            sequencer.Activate(source);
        }
        
        public void Cancel() 
            => sequencer.Cancel();

        public void KnockBackRegisterActive()
        {
            if (Cb.CurrentBehaviour is not null && Cb.BehaviourMask != BehaviourMask)
            {
                Cb.CurrentBehaviour.Cancel();
            }

            Cb.CurrentBehaviour = this;
        }

        public void KnockBackRotateActive(Vector3 source)
        {
            Cb.Rotate(source);
        }
        
        public void KnockBackAnimationActive(Vector3 source)
        {
            Cb.Animating.Hit();
        }

        public void KnockBackCancel() => Cb.Stop();
        public void KnockBackComplete() => Cb.Stop();
        
        
        private async UniTask AwaitKnockBack(Vector3 destination)
        {
            await Cb.Pathfinding.KnockBack(destination);
        }

        private void Awake()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveSection.AddAwait("AwaitKnockBack", AwaitKnockBack);
        }
        
        private void OnDestroy()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer = GetComponentInChildren<KnockBackSequencer>();
        }
#endif
    }
}
