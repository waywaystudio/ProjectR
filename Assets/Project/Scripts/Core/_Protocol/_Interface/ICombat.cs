using UnityEngine;

namespace Core
{
    public interface IObjectName
    {
        string Name { get; }
        GameObject Object { get; }
    }
    
    public interface IOriginalProvider
    {
        ICombatProvider Provider { get; }
    }
    
    public interface IActionSender : IOriginalProvider, IDataIndexer
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }
    }

    public interface ICombatTable : IActionSender
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }
        
        StatTable StatTable { get; }
    }
    
    public interface IStatusEffect : IActionSender
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }

        StatusEffectType Type { get; }
        float Duration { get; }
        FloatEvent ProcessTime { get; }
        ActionTable OnEnded { get; }

        void Active(ICombatTaker taker);
        void OnOverride();
    }

    public interface ICombatEntity : ICombatTable
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }
        // # IStatEntry StatTable { get; }
        
        IDynamicStatEntry DynamicStatEntry { get; }
    }

    public interface ICombatProvider : ICombatEntity, IObjectName
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }
        // # StatTable StatTable { get; }
        // # IDynamicStatEntry DynamicStatEntry { get; }
        // # string Name { get; }
        // # GameObject Object { get; }

        ActionTable<CombatEntity> OnProvideCombat { get; }
        ActionTable<StatusEffectEntity> OnProvideStatusEffect { get; }
    }
    
    public interface ICombatTaker : ICombatEntity, IObjectName
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }
        // # StatTable StatTable { get; }
        // # IDynamicStatEntry DynamicStatEntry { get; }
        // # string Name { get; }
        // # GameObject Object { get; }

        RoleType Role { get; }
        Transform DamageSpawn { get; }

        ActionTable<CombatEntity> OnTakeCombat { get; }
        ActionTable<StatusEffectEntity> OnTakeStatusEffect { get; }

        void Dead();
        CombatEntity TakeDamage(ICombatTable combatTable);
        CombatEntity TakeHeal(ICombatTable combatTable);
        StatusEffectEntity TakeBuff(IStatusEffect statusEffect);
        StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect);
    }

    public interface ICombatExecutor : ICombatProvider, ICombatTaker
    {
        // # ICombatProvider Provider { get; }
        // # DataIndex ActionCode { get; }
        // # StatTable StatTable { get; }
        // # IDynamicStatEntry DynamicStatEntry { get; }
        // # string Name { get; }
        // # GameObject Object { get; }
        
        // # ActionTable<CombatEntity> OnProvideCombat { get; }
        // # ActionTable<CombatEntity> OnTakeCombat { get; }
        // # ActionTable<StatusEffectEntity> OnProvideStatusEffect { get; }
        // # ActionTable<CombatEntity> OnTakeStatusEffect { get; }

        // # void Dead();
        // # CombatEntity TakeDamage(ICombatTable entity);
        // # CombatEntity TakeHeal(ICombatTable entity);
        // # StatusEffectEntity TakeBuff(INewStatusEffect entity);
        // # StatusEffectEntity TakeDeBuff(INewStatusEffect entity);
    }
}