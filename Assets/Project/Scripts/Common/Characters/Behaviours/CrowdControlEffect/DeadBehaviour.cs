using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours.CrowdControlEffect
{
    public class DeadBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private DeadSequencer sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.Dead;
        public CharacterActionMask IgnorableMask => CharacterActionMask.DeadIgnoreMask;
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask;
        

        public void Dead()
        {
            if (!sequencer.IsAbleToActive) return;

            sequencer.Active();
        }
        
        public void Cancel() 
            => sequencer.Cancel();

        // TODO. 여기가 맞나;
        public void AddReward(string key, Action action)
        {
            sequencer.Complete.Add(key, action);
        }
        
        public void DeadRegisterActive()
        {
            if (Cb.CurrentBehaviour is not null && Cb.BehaviourMask != BehaviourMask)
            {
                Cb.CurrentBehaviour.Cancel();
            }

            Cb.CurrentBehaviour = this;
        }

        public void DeadQuitPathfindingActive()
        {
            Cb.Pathfinding.Quit();
        }
        
        
        private async UniTask DeadAnimationActive()
        {
            await Cb.Animating.DeadAwait();
        }

        private void Awake()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveSection.AddAwait("DeadAnimation", DeadAnimationActive);
        }
        
        private void OnDestroy()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer = GetComponentInChildren<DeadSequencer>();
        }
#endif
    }
}
