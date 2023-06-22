using Common.Characters.Behaviours;
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
        IDynamicStatEntry DynamicStatEntry { get; }
        ActionMask BehaviourMask { get; }
    }

    public interface ICombatProvider : ICombatEntity, IObjectName
    {
        // + string Name { get; }
        // + GameObject gameObject { get; }
        // + Vector3 Position { get; }
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }

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
        DeadBehaviour DeadBehaviour { get; }
        SkillBehaviour SkillBehaviour { get; }
        ActionTable<CombatEntity> OnDamageTaken { get; }
        ActionTable<CombatEntity> OnHealTaken { get; }

        void Run(Vector3 destination);
        void Rotate(Vector3 lookTarget);
        void Stop();
        void Stun(float duration);
        void KnockBack(Vector3 source, float distance, float duration);
        void Dead();
        void TakeStatusEffect(StatusEffect effect);
        void DispelStatusEffect(StatusEffect effect);
    }

    public interface ICombatExecutor : ICombatProvider, ICombatTaker, IDataIndexer
    {
        // + DataIndex ActionCode { get; }
        
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }
        // + RoleType Role { get; }
        // + ICharacterAction CharacterAction { get; }
        
        // + ActionTable<CombatEntity> OnDamageProvided { get; }
        // + ActionTable<CombatEntity> OnHealProvided { get; }
        // + ActionTable<StatusEffectEntity> OnDeBuffProvided { get; }
        // + ActionTable<StatusEffectEntity> OnBuffProvided { get; }
        // + ActionTable<CombatEntity> OnDamageTaken { get; }
        // + ActionTable<CombatEntity> OnHealTaken { get; }
        // + ActionTable<StatusEffectEntity> OnDeBuffTaken { get; }
        // + ActionTable<StatusEffectEntity> OnBuffTaken { get; }
    }
}