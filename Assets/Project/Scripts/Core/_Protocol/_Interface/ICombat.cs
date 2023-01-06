using UnityEngine;

namespace Core
{
    public interface IOrigin
    {
        ICombatProvider Provider { get; }
    }
    
    public interface ICombatTable
    {
        IDynamicStatEntry DynamicStatEntry { get; }
        StatTable StatTable { get; }
    }

    public interface ICombatEntity : IOrigin, ICombatTable
    {
        // + ICombatProvider Provider { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + IStatEntry StatEntry { get; }
    }
    
    public interface IActionSender : IOrigin
    {
        // + ICombatProvider Provider { get; }

        IDCode ActionCode { get; }
    }

    public interface IObjectName
    {
        string Name { get; }
        GameObject Object { get; }
    }

    public interface ICombatProvider : ICombatEntity, IActionSender, IObjectName
    {
        // + ICombatProvider Provider { get; }
        // + Status Status { get; }
        // + StatTable StatTable { get; }
        // + IDCode ActionCode { get; }
        // + string Name { get; }
        // + GameObject Object { get; }
        
        ActionTable<CombatLog> OnCombatActive { get; }
    }
    
    public interface ICombatTaker : ICombatTable, IObjectName
    {
        // + Status Status { get; }
        // + StatTable StatTable { get; }
        // + string Name { get; }
        // + GameObject Object { get; }
        
        ActionTable<ICombatEntity> OnTakeStatusEffect { get; }
        ActionTable<IDCode> OnDispelStatusEffect { get; }
        ActionTable<CombatLog> OnCombatPassive { get; }

        void TakeDamage(ICombatEntity provider);
        void TakeSpell(ICombatEntity provider);
        void TakeHeal(ICombatEntity provider);
        void TakeStatusEffect(ICombatEntity provider);

        void DispelStatusEffect(IDCode code);
    }

    public interface ICombatExecutor : ICombatProvider, ICombatTaker
    {
        // + ICombatProvider Provider { get; }
        // + Status Status { get; }
        // + StatTable StatTable { get; }
        // + IDCode ActionCode { get; }
        // + string Name { get; }
        // + GameObject Object { get; }
        
        // + ActionTable<CombatLog> OnCombatActive { get; }
        // + ActionTable<ICombatEntity> OnTakeStatusEffect { get; }
        // + ActionTable<IDCode> OnDispelStatusEffect { get; }
        // + ActionTable<CombatLog> OnCombatPassive { get; }
        
        // + void TakeDamage(ICombatEntity provider);
        // + void TakeSpell(ICombatEntity provider);
        // + void TakeHeal(ICombatEntity provider);
        // + void TakeStatusEffect(ICombatEntity provider);
        // + void DispelStatusEffect(IDCode code);
    }
}