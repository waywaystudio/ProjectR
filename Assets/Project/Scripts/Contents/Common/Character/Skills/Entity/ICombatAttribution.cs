using Core;

namespace Common.Character.Skills.Entity
{
    public interface ICombatAttribution :
        IDamageEntity,
        ICoolTimeEntity,
        IRangeEntity,
        ICastingEntity,
        ITargetEntity
    {}
    
    public interface IDamageEntity
    {
        double Value { get; set; }
        float AdditionalValue { get; set; }
        float CriticalChance { get; set; }
        float HitChance { get; set; }
        float AdditionalAggro { get; set; }
    }
    
    public interface ICoolTimeEntity
    {
        float CoolTime { get; set; }
        float CoolTimeTick { get; set; }
    }
    
    public interface IRangeEntity
    {
        float Range { get; set; }
        UnityEngine.Vector3 TargetPosition { get; set; }
    }

    public interface ICastingEntity
    {
        float CastingTime { get; set; }
        float CastingTick { get; set; }
        System.Action OnStart { get; set; }
        System.Action OnBroken { get; set; }
        System.Action OnCompleted { get; set; }
    }

    public interface ITargetEntity
    {
        int TargetCount { get; set; }
        UnityEngine.LayerMask TargetLayer { get; set; }
        System.Collections.Generic.List<ICombatTaker> TargetList { get; set; }
    }
    
    public interface IReadyRequired
    {
        bool IsReady { get; }
    }
    
    public interface IUpdateRequired
    {
        void UpdateStatus();
    }
}
