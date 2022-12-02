namespace Common.Character.Skills.Entity
{
    public interface ISkillEntity :
        IDamageEntity,
        ICoolTimeEntity,
        IRangeEntity,
        ICastingEntity
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
        UnityEngine.GameObject Target { get; set; }
    }

    public interface ICastingEntity
    {
        float CastingTime { get; set; }
        float CastingTick { get; set; }
        System.Action OnStart { get; set; }
        System.Action OnBroken { get; set; }
        System.Action OnCompleted { get; set; }
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
