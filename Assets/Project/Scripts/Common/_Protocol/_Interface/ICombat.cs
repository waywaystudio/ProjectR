using Common.Characters.Behaviours;
using UnityEngine;

namespace Common
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
        // + GameObject Object { get; }

        ActionTable<CombatEntity> OnProvideDamage { get; }
        ActionTable<CombatEntity> OnProvideHeal { get; }
        ActionTable<StatusEffectEntity> OnProvideDeBuff { get; }
        ActionTable<StatusEffectEntity> OnProvideBuff { get; }
    }
    
    public interface ICombatTaker : ICombatEntity, IObjectName
    {
        // + StatTable StatTable { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + string Name { get; }
        // + GameObject Object { get; }

        RoleType Role { get; }
        SkillBehaviour SkillBehaviour { get; }
        Transform DamageSpawn { get; }
        Transform StatusEffectHierarchy { get; }
        ActionTable<CombatEntity> OnTakeDamage { get; }
        ActionTable<CombatEntity> OnTakeHeal { get; }
        ActionTable<StatusEffectEntity> OnTakeDeBuff { get; }
        ActionTable<StatusEffectEntity> OnTakeBuff { get; }

        CombatEntity TakeDamage(ICombatTable combatTable);
        CombatEntity TakeHeal(ICombatTable combatTable);
        StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect);
        StatusEffectEntity TakeBuff(IStatusEffect statusEffect);

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
        // + GameObject Object { get; }
        // + RoleType Role { get; }
        // + ICharacterAction CharacterAction { get; }
        
        // + ActionTable<CombatEntity> OnProvideDamage { get; }
        // + ActionTable<CombatEntity> OnProvideHeal { get; }
        // + ActionTable<StatusEffectEntity> OnProvideDeBuff { get; }
        // + ActionTable<StatusEffectEntity> OnProvideBuff { get; }
        // + ActionTable<CombatEntity> OnTakeDamage { get; }
        // + ActionTable<CombatEntity> OnTakeHeal { get; }
        // + ActionTable<StatusEffectEntity> OnTakeDeBuff { get; }
        // + ActionTable<StatusEffectEntity> OnTakeBuff { get; }

        // + CombatEntity TakeDamage(ICombatTable combatTable);
        // + CombatEntity TakeHeal(ICombatTable combatTable);
        // + StatusEffectEntity TakeDeBuff(IStatusEffect statusEffect);
        // + StatusEffectEntity TakeBuff(IStatusEffect statusEffect);
    }
}