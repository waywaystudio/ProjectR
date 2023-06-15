using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class KnockBackBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<Vector3> sequencer;        
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.KnockBack;
        
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.KnockBackIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.KnockBackIgnoreMask;
        

        public void KnockBack(Vector3 source, float distance, float duration)
        {
            if (!sequencer.IsAbleToActive) return;

            Cb.Pathfinding.KnockBack(source, distance, duration, sequencer.Complete);
            sequencer.Active(source);
        }
        
        public void Cancel() => sequencer.Cancel();


        private void OnEnable()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveParamAction.Add("Rotate", Cb.Rotate);
            sequencer.ActiveAction.Add("CommonKnockBackAction", () =>
            {
                if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
                    cb.CurrentBehaviour.Cancel();

                cb.CurrentBehaviour = this;
                Cb.Animating.Hit();
            });
            
            sequencer.EndAction.Add("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
