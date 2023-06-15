using Cysharp.Threading.Tasks;
using Sequences;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class KnockBackBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private Sequencer<Vector3> sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.KnockBack;
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.KnockBackIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.KnockBackIgnoreMask;
        

        public void KnockBack(Vector3 source, float distance, float duration)
        {
            if (!sequencer.IsAbleToActive) return;
            
            Cb.Pathfinding.SetKnockProperty(distance, duration);
            sequencer.ActivateSequence(source);
        }
        
        public void Cancel() 
            => sequencer.Cancel();
        
        public void KnockBackRotateActiveParam(Vector3 source)
        {
            Cb.Rotate(source);
        }

        public void KnockBackRegisterActive()
        {
            if (Cb.CurrentBehaviour is not null && Cb.BehaviourMask != BehaviourMask)
            {
                Cb.CurrentBehaviour.Cancel();
            }

            Cb.CurrentBehaviour = this;
        }
        
        public void KnockBackAnimationActive()
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
            sequencer.ActiveParamSection.AddAwait("AwaitKnockBack", AwaitKnockBack);
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
