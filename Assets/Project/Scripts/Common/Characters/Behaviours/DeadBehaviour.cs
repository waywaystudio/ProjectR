using System;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class DeadBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer sequencer;

        public ActionMask BehaviourMask => ActionMask.Dead;
        public SequenceBuilder SequenceBuilder { get; private set; }
        public SequenceInvoker SequenceInvoker { get; private set; }
        
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
            SequenceBuilder.Add(Section.Complete, key, action);
        }


        private void OnEnable()
        {
            SequenceInvoker = new SequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder(sequencer);
            SequenceBuilder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .Add(Section.Active,"CancelPreviousBehaviour", () => Cb.CurrentBehaviour?.TryToOverride(this))
                           .Add(Section.Active,"SetCurrentBehaviour", () => Cb.CurrentBehaviour = this)
                           .Add(Section.Active,"PlayAnimation", () => Cb.Animating.Dead(SequenceInvoker.Complete))
                           .Add(Section.Active,"Cb.Pathfinding.Quit", Cb.Pathfinding.Quit);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
