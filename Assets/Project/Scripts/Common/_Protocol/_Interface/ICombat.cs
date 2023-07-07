using Common.Characters.Behaviours;
using Common.Particles;
using Common.StatusEffects;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Common
{
    public interface IObjectName
    {
        string Name { get; }
        GameObject gameObject { get; }
        Vector3 Position { get; }
        Vector3 Forward { get; }
    }
    
    public interface IOriginalProvider
    {
        ICombatProvider Provider { get; }
    }
    
    public interface IActionSender : IOriginalProvider, IDataIndexer
    {
        // + ICombatProvider Provider { get; }
        // + DataIndex ActionCode { get; }
    }

    public interface ICombatEntity
    {
        StatTable StatTable { get; }
        AliveValue Alive { get; }
        HpValue Hp { get; }
        ResourceValue Resource { get; }
        ShieldValue Shield { get; }
        StatusEffectTable StatusEffectTable { get; }

        ActionMask BehaviourMask { get; }
        
        // IDynamicStatEntry DynamicStatEntry { get; }
        // StatTable StatTable { get; }
    }

    public interface ICombatProvider : ICombatEntity, IObjectName
    {
        // + string Name { get; }
        // + GameObject gameObject { get; }
        // + Vector3 Position { get; }
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }
        SkillBehaviour SkillBehaviour { get; }

        ActionTable<CombatEntity> OnDamageProvided { get; }
        ActionTable<CombatEntity> OnHealProvided { get; }
    }
    
    public interface ICombatTaker : ICombatEntity, IObjectName
    {
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }

        CharacterMask CombatClass { get; }
        Transform DamageSpawn { get; }
        Transform StatusEffectHierarchy { get; }
        
        StopBehaviour StopBehaviour { get; }
        RunBehaviour RunBehaviour { get; }
        StunBehaviour StunBehaviour { get; }
        KnockBackBehaviour KnockBackBehaviour { get; }
        DrawBehaviour DrawBehaviour { get; }
        DeadBehaviour DeadBehaviour { get; }
        ActionTable<CombatEntity> OnDamageTaken { get; }
        ActionTable<CombatEntity> OnHealTaken { get; }

        void Run(Vector3 destination);
        void Rotate(Vector3 lookTarget);
        void Stop();
        void Stun(float duration);
        void KnockBack(Vector3 source, float distance, float duration);
        void Draw(Vector3 source, float duration);
        void Dead();
        void TakeStatusEffect(StatusEffect effect);
        void DispelStatusEffect(DataIndex effectIndex);
        void PlayEffect(IActionSender actionSender, ParticleComponent particle);
    }

    public interface ICombatExecutor : ICombatProvider, ICombatTaker, IDataIndexer
    {
        
    }
}