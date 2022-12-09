using System.Collections.Generic;
using Core;

namespace Common.Character.Operation.Combating
{
    public interface ICombatAttribution :
        IDamageEntity,
        ICoolTimeEntity,
        ICastingEntity,
        ITargetEntity
    {}
    
    public interface IDamageEntity
    {
        double CombatValue { get; set; }
        float AdditionalCombat { get; set; }
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

    public interface ITargetEntity : IRangeEntity
    {
        int TargetCount { get; set; }
        UnityEngine.LayerMask TargetLayer { get; set; }
        ICombatTaker Target { get; set; }
        List<ICombatTaker> TargetList { get; set; }
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
