using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public abstract class ActionBehaviour : MonoBehaviour, IConditionalSequence
    {
        private CharacterBehaviour cb;
        
        [ShowInInspector] public abstract CharacterActionMask BehaviourMask { get; }
        [ShowInInspector] public abstract CharacterActionMask IgnorableMask { get; }
        [ShowInInspector] public ConditionTable Conditions { get; } = new();
        [ShowInInspector] public ActionTable OnActivated { get; } = new();
        [ShowInInspector] public ActionTable OnCanceled { get; } = new();
        [ShowInInspector] public ActionTable OnCompleted { get; } = new();
        [ShowInInspector] public ActionTable OnEnded { get; } = new();
        
        protected CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public virtual void Cancel() => OnCanceled.Invoke();
        protected virtual void Complete() => OnCompleted.Invoke();

        protected bool CanOverrideToCurrent => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask;

        protected void RegisterBehaviour(CharacterBehaviour cb)
        {
            if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
            {
                // if (cb.Role == RoleType.Tank)
                // {
                //     Debug.Log($"Current:{cb.BehaviourMask.ToString()} New:{this.BehaviourMask.ToString()}");
                // }
                
                cb.CurrentBehaviour.Cancel();
            }
                
            
            cb.CurrentBehaviour = this;
        }

        protected void Dispose()
        {
            Conditions.Clear();
            OnActivated.Clear();
            OnCanceled.Clear();
            OnCompleted.Clear();
            OnEnded.Clear();
        }
    }
}
