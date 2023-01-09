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
        // + IStatEntry StatTable { get; }
    }
    
    public interface IActionSender : IOrigin
    {
        // + ICombatProvider Provider { get; }

        DataIndex ActionCode { get; }
    }

    public interface IObjectName
    {
        string Name { get; }
        GameObject Object { get; }
    }

    public interface ICombatProvider : ICombatEntity, IActionSender, IObjectName
    {
        // + ICombatProvider Provider { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + StatTable StatTable { get; }
        // + IDCode ActionCode { get; }
        // + string Name { get; }
        // + GameObject Object { get; }
        
        ActionTable<CombatLog> OnCombatActive { get; }
    }
    
    public interface ICombatTaker : ICombatTable, IObjectName
    {
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + StatTable StatTable { get; }
        // + string Name { get; }
        // + GameObject Object { get; }
        
        ActionTable<IStatusEffect> OnTakeStatusEffect { get; }
        ActionTable<DataIndex> OnDispelStatusEffect { get; }
        ActionTable<CombatLog> OnCombatPassive { get; }

        void TakeDamage(ICombatEntity entity);
        void TakeSpell(ICombatEntity entity);
        void TakeHeal(ICombatEntity entity);
        
        void TakeStatusEffect(IStatusEffect entity);
        void DispelStatusEffect(DataIndex code);
    }

    public interface ICombatExecutor : ICombatProvider, ICombatTaker
    {
        // + ICombatProvider Provider { get; }
        // + IDynamicStatEntry DynamicStatEntry { get; }
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