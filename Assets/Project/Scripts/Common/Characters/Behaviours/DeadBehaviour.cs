using System;
using Cysharp.Threading.Tasks;
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
            SequenceBuilder.Add(SectionType.Complete, key, action);
        }


        private void OnEnable()
        {
            SequenceInvoker = new SequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder(sequencer);
            SequenceBuilder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .Add(SectionType.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                           .Add(SectionType.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .Add(SectionType.Active,"PlayAnimation", () => Cb.Animating.Dead(SequenceInvoker.Complete))
                           .Add(SectionType.Active,"Cb.Pathfinding.Quit", Cb.Pathfinding.Quit);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
