using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Characters.Behaviours
{
    public class DeadBehaviour : MonoBehaviour, IActionBehaviour
    {
        public ActionMask BehaviourMask => ActionMask.Dead;
        public Sequencer Sequence { get; } = new();
        public SequenceBuilder Builder { get; private set; }
        public SequenceInvoker Invoker { get; private set; }
        
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public void Dead()
        {
            if (!Invoker.IsAbleToActive) return;
            
            Invoker.Active();
        }

        public void Cancel() => Invoker.Cancel();

        // TODO. 여기가 맞나;
        public void AddReward(string key, Action action)
        {
            Builder.Add(Section.Complete, key, action);
        }


        private void OnEnable()
        {
            Invoker = new SequenceInvoker(Sequence);
            Builder = new SequenceBuilder(Sequence);
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                .Add(Section.Active,"CancelPreviousBehaviour", () => Cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active,"SetCurrentBehaviour", () => Cb.CurrentBehaviour = this)
                .Add(Section.Active,"PlayAnimation", () => Cb.Animating.Dead(Invoker.Complete))
                .Add(Section.Active,"Cb.Pathfinding.Quit", Cb.Pathfinding.Quit);
        }

        private void OnDisable()
        {
            Sequence.Clear();
        }
    }
}
