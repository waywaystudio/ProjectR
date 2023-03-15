using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public abstract class ActionBehaviour : MonoBehaviour
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
        
        protected void RegisterBehaviour(CharacterBehaviour cb)
        {
            if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
                cb.CurrentBehaviour.OnCanceled.Invoke();
            
            cb.CurrentBehaviour = this;
        }
    }
}
