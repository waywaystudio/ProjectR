using System;
using UnityEngine;
using CSBuilder = Common.CombatSequenceBuilder;

namespace Common
{
    [Serializable]
    public class CombatSequence : Sequencer
    {
        public ActionTable<Vector3> ApplyAction { get; } = new();
        public ActionTable<Vector3> FireAction { get; } = new();
        public ActionTable<Vector3> SubFireAction { get; } = new();
        public ActionTable<ICombatTaker> HitAction { get; } = new();
        public ActionTable<ICombatTaker> SubHitAction { get; } = new();
        
        public override void Clear()
        {
            base.Clear();
        
            ApplyAction.Clear();
            FireAction.Clear();
            SubFireAction.Clear();
            HitAction.Clear();
            SubHitAction.Clear();
        }
    }

    [Serializable] 
    public class CombatSequenceBuilder
    {
        private readonly CombatSequence sequence;

        public CombatSequenceBuilder(CombatSequence sequence) => this.sequence = sequence;
        
        public CSBuilder AddCondition(string key, Func<bool> condition) { sequence.Condition.Add(key, condition); return this; }
        public CSBuilder AddApplying(string key, Action<Vector3> action) { sequence.ApplyAction.Add(key, action); return this; }
        public CSBuilder AddFire(string key, Action<Vector3> action) { sequence.FireAction.Add(key, action); return this; }
        public CSBuilder AddHit(string key, Action<ICombatTaker> action) { sequence.HitAction.Add(key, action); return this; }
        public CSBuilder AddSubFire(string key, Action<Vector3> action) { sequence.SubFireAction.Add(key, action); return this; }
        public CSBuilder AddSubHit(string key, Action<ICombatTaker> action) { sequence.SubHitAction.Add(key, action); return this; }
        public CSBuilder Add(Section type, string key, Action action) { sequence[type].Add(key, action); return this; }
        public CSBuilder AddIf(bool condition, Section type, string key, Action action) { if (condition) Add(type, key, action); return this; }

        public CSBuilder RemoveCondition(string key) { sequence.Condition.Remove(key); return this; }
        public CSBuilder RemoveApplying(string key) { sequence.ApplyAction.Remove(key); return this; }
        public CSBuilder RemoveFire(string key) { sequence.FireAction.Remove(key); return this; }
        public CSBuilder RemoveHit(string key) { sequence.HitAction.Remove(key); return this; }
        public CSBuilder RemoveSubFire(string key) { sequence.SubFireAction.Remove(key); return this; }
        public CSBuilder RemoveSubHit(string key) { sequence.SubHitAction.Remove(key); return this; }
        public CSBuilder Remove(Section type, string key) { sequence[type].Remove(key); return this; }
    }

    [Serializable] 
    public class CombatSequenceInvoker
    {
        private readonly CombatSequence sequence;
    
        public CombatSequenceInvoker(CombatSequence sequence) => this.sequence = sequence;
    
        public bool IsAbleToActive => sequence.Condition == null || sequence.Condition.IsAllTrue;
        public bool IsActive { get; set; }
        public bool IsEnd { get; private set; } = true;
        
        public void Active(Vector3 value)
        {
            // Active 가 ActiveParam보다 우선되게 설정. RunBehaviour 참조.
            Active();
            sequence.ApplyAction.Invoke(value);
        }
        
        public void Active()
        {
            IsEnd    = false;
            IsActive = true;
            
            sequence[Section.Active].Invoke();
        
            // Handle Active Just once and than disappear Action.
            if (sequence.Table.Remove(Section.ActiveOnce, out var onceAction))
            {
                onceAction.Invoke();
            }
        }

        public void Cancel()
        {
            IsActive = false;
        
            sequence[Section.Cancel].Invoke();
            End();
        }
        
        public void Complete()
        {
            IsActive = false;
        
            sequence[Section.Complete].Invoke();
            End();
        }

        public void End()
        {
            IsEnd = true;
        
            sequence[Section.End].Invoke();
        }
    
        public void Execute() => sequence[Section.Execute].Invoke();
        public void ExtraAction() => sequence[Section.Extra].Invoke();
        public void Override() => sequence[Section.Override].Invoke();
        public void Release() => sequence[Section.Release].Invoke();
        public void Fire(Vector3 position) => sequence.FireAction.Invoke(position);
        public void Hit(ICombatTaker taker) => sequence.HitAction.Invoke(taker);
        public void SubFire(Vector3 position) => sequence.SubFireAction.Invoke(position);
        public void SubHit(ICombatTaker taker) => sequence.SubHitAction.Invoke(taker);
    }
}
