using UnityEngine;

namespace Core
{
    public interface IOriginalProvider
    {
        ICombatProvider Provider { get; }
    }
    
    public interface ICombatTable : IOriginalProvider
    {
        // + ICombatProvider Provider { get; }
        StatTable StatTable { get; }
    }

    public interface ICombatEntity : ICombatTable
    {
        // + ICombatProvider Provider { get; }
        // + IStatEntry StatTable { get; }
        
        IDynamicStatEntry DynamicStatEntry { get; }
    }
    
    public interface IActionSender : IOriginalProvider
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
    
    public interface ICombatTaker : ICombatEntity, IObjectName
    {
        // + IDynamicStatEntry DynamicStatEntry { get; }
        // + StatTable StatTable { get; }
        // + string Name { get; }
        // + GameObject Object { get; }

        RoleType Role { get; }
        
        ActionTable<IStatusEffect> OnTakeStatusEffect { get; }
        ActionTable<DataIndex> OnDispelStatusEffect { get; }
        ActionTable<CombatLog> OnCombatPassive { get; }

        void Dead();
        void TakeDamage(ICombatTable entity);
        void TakeSpell(ICombatTable entity);
        void TakeHeal(ICombatTable entity);
        
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