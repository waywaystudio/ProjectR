using Common.Characters.Behaviours;
using Common.Skills;
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
        ActionMask BehaviourMask { get; }
        
        StatTable StatTable { get; }
        AliveValue Alive { get; }
        HpValue Hp { get; }
        ResourceValue Resource { get; }
        ShieldValue Shield { get; }
        StatusEffectTable StatusEffectTable { get; }
    }

    public interface ICombatProvider : IObjectName, ICombatEntity 
    {
        // + string Name { get; }
        // + GameObject gameObject { get; }
        // + Vector3 Position { get; }
        // + Vector3 Forward { get; }
        
        // + ActionMask BehaviourMask { get; }
        // + StatTable StatTable { get; }
        // + AliveValue Alive { get; }
        // + HpValue Hp { get; }
        // + ResourceValue Resource { get; }
        // + ShieldValue Shield { get; }
        // + StatusEffectTable StatusEffectTable { get; }

        SkillTable SkillTable { get; }
        ActionTable<CombatLog> OnCombatProvided { get; }
    }
    
    public interface ICombatTaker : IObjectName, ICombatEntity, ICombatBehaviour 
    {
        // + string Name { get; }
        // + GameObject gameObject { get; }
        // + Vector3 Position { get; }
        // + Vector3 Forward { get; }
        
        // + ActionMask BehaviourMask { get; }
        // + StatTable StatTable { get; }
        // + AliveValue Alive { get; }
        // + HpValue Hp { get; }
        // + ResourceValue Resource { get; }
        // + ShieldValue Shield { get; }
        // + StatusEffectTable StatusEffectTable { get; }
        
        // + StopBehaviour StopBehaviour { get; }
        // + RunBehaviour RunBehaviour { get; }
        // + StunBehaviour StunBehaviour { get; }
        // + KnockBackBehaviour KnockBackBehaviour { get; }
        // + DrawBehaviour DrawBehaviour { get; }
        // + DeadBehaviour DeadBehaviour { get; }

        CharacterMask CombatClass { get; }
        Transform CombatStatusHierarchy { get; }
        ActionTable<CombatLog> OnCombatTaken { get; }
        void TakeStatusEffect(StatusEffect effect);
        void DispelStatusEffect(DataIndex effectIndex);
        
        Transform Preposition(PrepositionType type);
    }

    public interface ICombatExecutor : ICombatProvider, ICombatTaker, IDataIndexer
    {
        
    }
}