using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class DeadBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.Dead;
        
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.DeadIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.DeadIgnoreMask;
        

        public void Dead()
        {
            if (!sequencer.IsAbleToActive) return;

            sequencer.Active();
        }
        
        public void Cancel() => sequencer.Cancel();

        // TODO. 여기가 맞나;
        public void AddReward(string key, Action action)
        {
            sequencer.CompleteAction.Add(key, action);
        }


        private void OnEnable()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveAction.Add("CommonStunAction", () =>
            {
                if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
                    cb.CurrentBehaviour.Cancel();

                cb.CurrentBehaviour = this;
                Cb.Animating.Dead(sequencer.Complete);
                Cb.Pathfinding.Quit();
            });
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
