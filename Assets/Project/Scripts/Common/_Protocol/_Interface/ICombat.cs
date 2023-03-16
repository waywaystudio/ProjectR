using UnityEngine;

namespace Common
{
    public interface IObjectName
    {
        string Name { get; }
        GameObject gameObject { get; }
        Transform transform { get; }
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

    public interface ICombatTable : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + DataIndex ActionCode { get; }
        
        StatTable StatTable { get; }

        void UpdateStatTable();
    }
    
    public interface IStatusEffect : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + DataIndex ActionCode { get; }

        StatusEffectType Type { get; }
        ICombatTaker Taker { get; }

        float Duration { get; }
        FloatEvent ProgressTime { get; }
        ActionTable OnEnded { get; }

        void OnOverride();
    }

    public interface ICombatEntity
    {
        StatTable StatTable { get; }
        IDynamicStatEntry DynamicStatEntry { get; }
    }

    public interface ICombatProvider : ICombatEntity, IObjectName
    {
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }

        ActionTable<CombatEntity> OnDamageProvided { get; }
        ActionTable<CombatEntity> OnHealProvided { get; }
        ActionTable<IStatusEffect> OnDeBuffProvided { get; }
        ActionTable<IStatusEffect> OnBuffProvided { get; }
    }
    
    public interface ICombatTaker : ICombatEntity, IObjectName
    {
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }

        RoleType Role { get; }
        Transform DamageSpawn { get; }
        Transform StatusEffectHierarchy { get; }
        
        ActionTable<CombatEntity> OnDamageTaken { get; }
        ActionTable<CombatEntity> OnHealTaken { get; }
        ActionTable<IStatusEffect> OnDeBuffTaken { get; }
        ActionTable<IStatusEffect> OnBuffTaken { get; }

        void TakeDamage(ICombatTable combatTable);
        void TakeHeal(ICombatTable combatTable);
        void TakeDeBuff(IStatusEffect statusEffect);
        void TakeBuff(IStatusEffect statusEffect);

        void Run(Vector3 destination);
        void Rotate(Vector3 lookTarget);
        void Stop();
        void Stun(float duration);
        void KnockBack(Vector3 source, float distance);
        void Dead();
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

        // + void TakeDamage(ICombatTable combatTable);
        // + void TakeHeal(ICombatTable combatTable);
        // + void TakeDeBuff(IStatusEffect statusEffect);
        // + void TakeBuff(IStatusEffect statusEffect);
    }
}