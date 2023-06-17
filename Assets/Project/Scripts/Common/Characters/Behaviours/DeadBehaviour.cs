using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class DeadBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer sequencer;

        public ActionMask BehaviourMask => ActionMask.Dead;
        public SequenceBuilder SequenceBuilder { get; } = new();
        public SequenceInvoker SequenceInvoker { get; } = new();
        
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void Dead()
        {
            if (!SequenceInvoker.IsAbleToActive) return;

            SequenceInvoker.Active();
        }

        public void Cancel() => SequenceInvoker.Cancel();

        // TODO. 여기가 맞나;
        public void AddReward(string key, Action action)
        {
            SequenceBuilder.AddComplete(key, action);
        }


        private void OnEnable()
        {
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .AddActive("CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToCancel(this))
                           .AddActive("SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .AddActive("PlayAnimation", () => Cb.Animating.Dead(SequenceInvoker.Complete))
                           .AddActive("Cb.Pathfinding.Quit", Cb.Pathfinding.Quit);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
