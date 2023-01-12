using System.Collections.Generic;
using Core;

namespace Character.Combat
{
    public interface ISKillModule : IModule
    {
    }

    public interface IReady
    {
        bool IsReady { get; }
    }

    public interface IDamageModule : ISKillModule
    {
        PowerValue DamageValue { get; }
    }

    public interface IHealModule : ISKillModule
    {
        PowerValue HealValue { get; }
    }

    public interface IResourceModule : ISKillModule
    {
        float Obtain { get; }
    }
    
    public interface ICoolModule : ISKillModule
    {
        float OriginalCoolTime { get; }
        Observable<float> RemainTime { get; } 
    }
    
    public interface ICastingModule : ISKillModule
    {
        float OriginalCastingTime { get; }
        float CastingTime { get; }
        float CastingProgress { get; }
    }
    
    public interface IProjectileModule : ISKillModule
    {
        DataIndex ProjectileID { get; }
        void Fire(ICombatTaker taker);
    }
    
    public interface IStatusEffectModule : ISKillModule
    {
        DataIndex StatusEffectID { get; }
        void Effectuate(ICombatTaker taker);
    }
    
    /* Currently, Only TargetSkill has Targeting TargetModule */
    public interface ITargetModule : ISKillModule
    {
        float Range { get; }
        ICombatTaker Target { get; }
        List<ICombatTaker> TargetList { get; }
    }
}
